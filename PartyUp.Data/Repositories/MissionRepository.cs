using PartyUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data.Repositories
{
    public class MissionRepository : GenericRepository<Mission>
    {
        public MissionRepository(ApplicationDbContext context) : base(context) { }
    }
}
