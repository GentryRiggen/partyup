using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PartyUp.Models;
using System.Threading.Tasks;
using PartyUp.Models.DTO;
using Microsoft.AspNet.SignalR.Hubs;
using PartyUp.Data;

namespace PartyUp.Hubs
{
    [HubName("events")]
    public class EventsHub : BaseHub
    {
        protected readonly static SignalRConnectionMappingToGroup _groupConnections = new SignalRConnectionMappingToGroup();

        public async Task JoinEventGroup(string groupId)
        {
            Tuple<int, int> ids = this.GetMissionAndEventId(groupId);
            Event e = await appDataFactory.Events.FindAsync(ids.Item2);
            EventDTO eventDTO = new EventDTO(e);
            User user = _connections.GetConnectedUser(Context.ConnectionId);
            int activeParticipants = e.EventParticipants.Where(ep => ep.Active = true).Count();
            if (activeParticipants < e.DesiredAmount ||
                e.EventParticipants.FirstOrDefault(u => u.User.Id == user.Id) != null)
            {
                // Add this user as a particpant to the DB (if they are not already part of the group
                if (e.EventParticipants.FirstOrDefault(u => u.User.Id == user.Id) == null && user.Id != e.Organizer.Id)
                {
                    e.EventParticipants.Add(new EventParticipant
                    {
                        Event = e,
                        User = user,
                        Active = true,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedOn = DateTime.UtcNow
                    });
                    try
                    {
                        await appDataFactory.SaveChangesAsync();
                    }
                    catch (Exception exceptioin)
                    {
                        Console.WriteLine(exceptioin);
                    }
                }

                // Now if they event participation has hit desired amount, remove it from the list
                if (e.EventParticipants != null && activeParticipants >= e.DesiredAmount)
                {
                    Clients.Group(ids.Item1.ToString()).removeEvent(eventDTO);
                }
                else if (e.EventParticipants != null && activeParticipants < e.DesiredAmount)
                {
                    // Tell the listeners the new looking for count
                    Clients.Group(ids.Item1.ToString()).updateLookingForCount(eventDTO, e.DesiredAmount - e.EventParticipants.Count());
                }

                // Add them to in memory list of grouping
                _groupConnections.Add(Context.ConnectionId, groupId);
                // Add it to SIGNALR Group
                await Groups.Add(Context.ConnectionId, groupId);

                // Tell caller they made it in
                Clients.Caller.successfullyJoinedGroup(eventDTO);

                // Tell everyone else that someone joined

                Clients.Group(groupId).userJoined(new UserDTO(user));
            }
            else
            {
                Clients.Caller.failedToJoinGroup(groupId);
            }
        }

        // Instant Messaging
        public void SendMessage(string message, string groupId)
        {
            User user = _connections.GetConnectedUser(Context.ConnectionId);
            Clients.Group(groupId).newMessage(user.FirstName, message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // If this person is part of a group, inform group they have left
            string groupId = _groupConnections.GetConnectedGroup(Context.ConnectionId);
            if (!String.IsNullOrEmpty(groupId))
            {
                User user = _connections.GetConnectedUser(Context.ConnectionId);
                Tuple<int, int> ids = this.GetMissionAndEventId(groupId);
                using (ApplicationDataFactory appData = new ApplicationDataFactory())
                {
                    Event e = appData.Events.Find(ids.Item2);
                    // Remove user from event participants in the DB
                    if (e.EventParticipants.FirstOrDefault(u => u.User.Id == user.Id) != null)
                    {
                        EventParticipant participant = e.EventParticipants.FirstOrDefault(u => u.User.Id == user.Id);
                        participant.Active = false;
                        appData.SaveChanges();
                    }

                    EventDTO eventDTO = new EventDTO(e);

                    int activeParticipants = (e.EventParticipants != null) ? 0 : e.EventParticipants.Where(ep => ep.Active = true).Count();
                    // If the organizer left, tell everyone to unlist event
                    if (e.Organizer.Id == user.Id)
                    {
                        Clients.Group(ids.Item1.ToString()).removeEvent(eventDTO);
                    }
                    // Now that the user has left, does this event need to be listed again?
                    else if (e.EventParticipants != null && activeParticipants < e.DesiredAmount)
                    {
                        Clients.Group(ids.Item1.ToString()).newHostedEvent(eventDTO);
                        // Tell the listeners the new looking for count
                        Clients.Group(ids.Item1.ToString()).updateLookingForCount(eventDTO, activeParticipants);
                    }
                }
                

                // Finally inform the event of people that a person has left
                Clients.Group(groupId).userLeft(new UserDTO(user));
            }


            return base.OnDisconnected(stopCalled);
        }


        // MISSION GROUP
        public void JoinMissionGroup(int missionId)
        {
            Groups.Add(Context.ConnectionId, missionId.ToString());
        }

        public async Task HostEvent(int missionId, EventDTO eventDetails)
        {
            User user = _connections.GetConnectedUser(Context.ConnectionId);
            Mission mission = await appDataFactory.Missions.FindAsync(missionId);

            if (user == null || mission == null)
            {
                return;
            }

            // Create Event
            Event missionEvent = new Event
            {
                Organizer = user,
                Mission = mission,
                Platform = appDataFactory.Platforms.getByName(eventDetails.Platform),
                DesiredAmount = eventDetails.DesiredAmount,
                Notes = eventDetails.Notes,
                HasMic = eventDetails.HasMic,
                Language = eventDetails.Language,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            };
            try
            {
                appDataFactory.Events.Add(missionEvent);
                // Not gonna wait for it, just fire and forget
                await appDataFactory.SaveChangesAsync();

                Event newEvent = await appDataFactory.Events.FindAsync(missionEvent.Id);
                EventDTO e = new EventDTO(newEvent);
                Clients.Caller.eventCreated(e);
                Clients.Group(missionId.ToString()).newHostedEvent(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private Tuple<int, int> GetMissionAndEventId(string groupId)
        {
            string[] arr = groupId.Split('-');
            if (arr.Length >= 2)
            {
                return Tuple.Create(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));
            }
            else
            {
                return Tuple.Create(-1, -1);
            }
        }
    }
}