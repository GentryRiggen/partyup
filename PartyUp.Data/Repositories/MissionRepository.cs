using PartyUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PartyUp.Data.Repositories
{
    public class MissionRepository : GenericRepository<Mission>
    {
        public MissionRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<Mission> GetAll()
        {
            return base.DbSet
                .OrderBy(m => m.Name)
                .Include("Community");
        }

        public override async Task<IQueryable<Mission>> GetAllAsync()
        {
            var all = await base.DbSet
                .OrderBy(m => m.Name)
                .Include("Community")
                .ToListAsync();
            return all.AsQueryable();
        }

        public override Mission Find(int id)
        {
            return this.DbSet
                .Where(m => m.Id == id)
                .Include("Community")
                .FirstOrDefault();
        }

        public override async Task<Mission> FindAsync(int id)
        {
            return await this.DbSet
                .Where(m => m.Id == id)
                .Include("Community")
                .FirstOrDefaultAsync();
        }

        public override Mission Find(int? id)
        {
            return this.DbSet
                .Where(m => m.Id == Convert.ToInt32(id))
                .Include("Community")
                .FirstOrDefault();
        }
    }
}
