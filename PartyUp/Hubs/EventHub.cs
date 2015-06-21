using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PartyUp.Models;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using PartyUp.Models.DTO;

namespace PartyUp.Hubs
{
    [HubName("event")]
    public class EventHub : BaseHub
    {
        protected readonly static SignalRConnectionMappingToGroup _groupConnections = new SignalRConnectionMappingToGroup();
        
        public async Task JoinEventGroup(string groupId)
        {
            Tuple<int, int> ids = this.GetMissionAndEventId(groupId);
            Event e = await appDataFactory.Events.FindAsync(ids.Item2);
            User user = _connections.GetConnectedUser(Context.ConnectionId);
            if (e.EventParticipants.Count() < e.DesiredAmount || e.EventParticipants.Where(u => u.Id == user.Id).First() != null)
            {
                // Add this user as a particpant to the DB (if they are not already part of the group
                if (e.EventParticipants.Where(u => u.Id == user.Id).First() == null)
                {
                    e.EventParticipants.Add(user);
                    await appDataFactory.SaveChangesAsync();
                }

                // Add them to in memory list of grouping
                _groupConnections.Add(Context.ConnectionId, groupId);
                // Add it to SIGNALR Group
                await Groups.Add(Context.ConnectionId, groupId);

                // Tell caller they made it in
                Clients.Caller.successfullyJoinedGroup(new EventDTO(e));

                // Tell everyone else that someone joined
                
                Clients.Group(groupId).userJoined(new UserDTO(user));
            }
            else
            {
                Clients.Caller.failedToJoinGroup(groupId);
            }
            
        }

        public void SendMessage(string message, string groupId)
        {
            User user = _connections.GetConnectedUser(Context.ConnectionId);
            Clients.Group(groupId).newMessage(user.FirstName, message);
        }

        private Tuple<int, int> GetMissionAndEventId(string groupId)
        {
            string[] arr = groupId.Split('-');
            if (arr.Length >= 2) {
                return Tuple.Create(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));
            } 
            else 
            {
                return Tuple.Create(-1, -1);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // If this person is part of a group, inform group they have left
            string groupId = _groupConnections.GetConnectedGroup(Context.ConnectionId);
            if (!String.IsNullOrEmpty(groupId))
            {
                User user = _connections.GetConnectedUser(Context.ConnectionId);
                Clients.Group(groupId).userLeft(new UserDTO(user));
            }
            

            return base.OnDisconnected(stopCalled);
        }
    }
}