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
        public UserDTO Organizer { get; set; }
        public MissionDTO Mission { get; set; }
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
            this.Organizer = new UserDTO(e.Organizer);
            this.Mission = new MissionDTO(e.Mission);
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
