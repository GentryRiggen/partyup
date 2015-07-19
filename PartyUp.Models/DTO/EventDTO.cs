using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models.DTO
{
    public class EventDTO : IDto<Event>
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("organizer")]
        public UserDTO Organizer { get; set; }

        [JsonProperty("mission")]
        public MissionDTO Mission { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("desiredAmount")]
        public int DesiredAmount { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("hasMic")]
        public bool HasMic { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("eventParticipants")]
        public IEnumerable<EventParticipantDTO> EventParticipants { get; set; }

        [JsonProperty("createdOn")]
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
            this.Platform = e.Platform.Name;
            this.DesiredAmount = e.DesiredAmount;
            List<EventParticipantDTO> participants = new List<EventParticipantDTO>();
            foreach (EventParticipant ep in e.EventParticipants)
            {
                participants.Add(new EventParticipantDTO(ep));
            }
            this.EventParticipants = participants;
            this.CreatedOn = e.CreatedOn;
            this.Notes = e.Notes;
            this.HasMic = e.HasMic;
            this.Language = e.Language;
        }

        public Event ToModel()
        {
            return new Event()
            {
                Id = this.Id,
                DesiredAmount = this.DesiredAmount,
                Notes = this.Notes,
                HasMic = this.HasMic,
                Language = this.Language
            };
        }

        public Event UpdateDbModel(Event entity)
        {
            entity.Notes = this.Notes;
            entity.HasMic = this.HasMic;
            entity.Language = this.Language;
            return entity;
        }
    }
}
