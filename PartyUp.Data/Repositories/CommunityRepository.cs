using PartyUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data.Repositories
{
    public class CommunityRepository : GenericRepository<Community>
    {
        public CommunityRepository(ApplicationDbContext context) : base(context) { }
    }
}
