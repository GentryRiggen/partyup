using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models.DTO
{
    public class EventParticipantDTO : IDto<EventParticipant>
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("eventId")]
        public int EventId { get; set; }

        [JsonProperty("user")]
        public UserDTO User { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("modifiedOn")]
        public DateTime ModifiedOn { get; set; }

        public EventParticipantDTO()
        {
            // NoOp
        }

        public EventParticipantDTO(EventParticipant e)
        {
            this.Id = e.Id;
            this.EventId = e.Event.Id;
            this.User = new UserDTO(e.User);
            this.Active = e.Active;
            this.CreatedOn = e.CreatedOn;
            this.ModifiedOn = e.ModifiedOn;
        }

        public EventParticipant ToModel()
        {
            return new EventParticipant()
            {
                Id = this.Id,
                Active = this.Active
            };
        }

        public EventParticipant UpdateDbModel(EventParticipant entity)
        {
            entity.Active = this.Active;
            return entity;
        }
    }
}
