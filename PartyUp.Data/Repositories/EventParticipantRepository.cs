using PartyUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data.Repositories
{
    public class EventParticipantRepository : GenericRepository<EventParticipant>
    {
        public EventParticipantRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<EventParticipant> GetAll()
        {
            return base.DbSet
                .OrderByDescending(e => e.CreatedOn)
                .Include("Event")
                .Include("User");
        }

        public override async Task<IQueryable<EventParticipant>> GetAllAsync()
        {
            var all = await base.DbSet
                .OrderByDescending(e => e.CreatedOn)
                .Include("Event")
                .Include("User")
                .ToListAsync();
            return all.AsQueryable();
        }

        public override EventParticipant Find(int id)
        {
            return this.DbSet
                .Where(c => c.Id == id)
                .Include("Event")
                .Include("User")
                .FirstOrDefault();
        }

        public override async Task<EventParticipant> FindAsync(int id)
        {
            return await this.DbSet
                .Where(c => c.Id == id)
                .Include("Event")
                .Include("User")
                .FirstOrDefaultAsync();
        }

        public override EventParticipant Find(int? id)
        {
            return this.DbSet
                .Where(c => c.Id == Convert.ToInt32(id))
                .Include("Event")
                .Include("User")
                .FirstOrDefault();
        }

        public async Task<IQueryable<EventParticipant>> GetAllByEventAsync(int eventId)
        {
            var all = await this.GetAllAsync();
            return all.Where(e => e.Event.Id == eventId);
        }
    }
}
