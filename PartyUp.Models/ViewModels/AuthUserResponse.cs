using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyUp.Models.DTO;

namespace PartyUp.Models.ViewModels
{
    public class AuthUserResponse
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<EventDTO> RecentlyHostedEvents { get; set; }
        public IEnumerable<EventDTO> RecentlyJoinedEvents { get; set; }

        public AuthUserResponse()
        {
            //noop
        }

        public AuthUserResponse(User u, string token, IEnumerable<string> roles, IEnumerable<Event> recentlyHostedEvents, IEnumerable<Event> recentlyJoinedEvents)
        {
            this.User = new UserDTO(u);
            this.Token = token;
            this.Roles = roles;

            List<EventDTO> recentlyHosted = new List<EventDTO>();
            foreach (Event e in recentlyHostedEvents)
            {
                recentlyHosted.Add(new EventDTO(e));
            }
            this.RecentlyHostedEvents = recentlyHosted;

            List<EventDTO> recentlyJoined = new List<EventDTO>();
            foreach (Event e in recentlyJoinedEvents)
            {
                recentlyJoined.Add(new EventDTO(e));
            }
            this.RecentlyJoinedEvents = recentlyJoined;
        }
    }
}
