using PartyUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data.Repositories
{
    public class PlatformRepository : GenericRepository<Platform>
    {
        public PlatformRepository(ApplicationDbContext context) : base(context) { }

        public Platform getByName(string name)
        {
            return this.DbSet
                .Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }
    }
}
