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
        [TokenAuth]
        [Route("api/missions/{missionId:int}/events")]
        public async Task<IEnumerable<EventDTO>> GetEvents(int missionId)
        {
            DateTime fiveMinutesAgo = DateTime.UtcNow;
            fiveMinutesAgo = fiveMinutesAgo.AddMinutes(-5);
            IQueryable<Event> query = await _appDataFactory.Events.GetAllByMission(missionId);
            IEnumerable<Event> events = query.Where(e => e.CreatedOn >= fiveMinutesAgo).ToList();
            List<EventDTO> eventsAsDTO = new List<EventDTO>();
            foreach (Event e in events)
            {
                if (e.Organizer.Id != User.Identity.Name)
                {
                    eventsAsDTO.Add(new EventDTO(e));
                }
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