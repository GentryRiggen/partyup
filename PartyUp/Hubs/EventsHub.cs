using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PartyUp.Models;
using System.Threading.Tasks;
using PartyUp.Models.DTO;
using Microsoft.AspNet.SignalR.Hubs;

namespace PartyUp.Hubs
{
    [HubName("events")]
    public class EventsHub : BaseHub
    {
        public void JoinMissionGroup(int missionId)
        {
            Groups.Add(Context.ConnectionId, missionId.ToString());
        }

        public async Task HostEvent(int missionId)
        {
            User user = _connections.GetConnectedUser(Context.ConnectionId);
            Mission mission = await appDataFactory.Missions.FindAsync(missionId);

            // Create Event
            Event missionEvent = new Event
            {
                Organizer = user,
                Mission = mission,
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
    }
}