using PartyUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data.Repositories
{
    public class EventRepository : GenericRepository<Event>
    {
        public EventRepository(ApplicationDbContext context) : base(context) { }
    }
}
