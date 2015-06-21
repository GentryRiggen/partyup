using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models.DTO
{
    public class EventDTO : IDto<Event>
    {
        public int Id { get; set; }
        public string OrganizerId { get; set; }
        public String OrganizerName { get; set; }
        public String OrganizerUserName { get; set; }
        public int MissionId { get; set; }
        public string MissionName { get; set; }
        public int DesiredAmount { get; set; }
        public IEnumerable<UserDTO> EventParticipants { get; set; }

        public DateTime CreatedOn { get; set; }

        public EventDTO()
        {
            // NoOp
        }

        public EventDTO(Event e)
        {
            this.Id = e.Id;
            this.OrganizerId = e.Organizer.Id;
            this.OrganizerName = e.Organizer.FirstName + " " + e.Organizer.LastName;
            this.OrganizerUserName = e.Organizer.UserName;
            this.MissionId = e.Mission.Id;
            this.MissionName = e.Mission.Name;
            this.DesiredAmount = e.DesiredAmount;
            List<UserDTO> participants = new List<UserDTO>();
            foreach (User u in e.EventParticipants)
            {
                participants.Add(new UserDTO(u));
            }
            this.EventParticipants = participants;
            this.CreatedOn = e.CreatedOn;
        }

        public Event ToModel()
        {
            return new Event()
            {
                Id = this.Id,
                DesiredAmount = this.DesiredAmount
            };
        }

        public Event UpdateDbModel(Event entity)
        {
            return entity;
        }
    }
}
