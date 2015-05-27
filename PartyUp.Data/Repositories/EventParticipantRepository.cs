using PartyUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data.Repositories
{
    public class EventParticipantRepository : GenericRepository<EventParticipant>
    {
        public EventParticipantRepository(ApplicationDbContext context) : base(context) { }
    }
}
