using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PartyUp.Data;
using PartyUp.Models;
using PartyUp.Filters;
using PartyUp.Models.DTO;

namespace PartyUp.Controllers
{
    public class EventsController : ApiController
    {
        private ApplicationDataFactory _appDataFactory = new ApplicationDataFactory();

        // GET: api/Events
        [Route("api/missions/{missionId:int}/events")]
        public async Task<IEnumerable<EventDTO>> GetEvents(int missionId)
        {
            IEnumerable<Event> events = await _appDataFactory.Events.GetAllByMission(missionId);
            List<EventDTO> eventsAsDTO = new List<EventDTO>();
            foreach (Event e in events)
            {
                eventsAsDTO.Add(new EventDTO(e));
            }
            return eventsAsDTO;
        }


        [TokenAuth]
        [Route("api/events/{eventId:int}")]
        public async Task<EventDTO> GetEvent(int eventId)
        {
            Event e = await _appDataFactory.Events.FindAsync(eventId);
            return new EventDTO(e);
        }
    }
}