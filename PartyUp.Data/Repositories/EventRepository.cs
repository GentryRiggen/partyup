using PartyUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data.Repositories
{
    public class EventRepository : GenericRepository<Event>
    {
        public EventRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<Event> GetAll()
        {
            return base.DbSet
                .OrderByDescending(e => e.CreatedOn)
                .Include("Platform")
                .Include("Organizer")
                .Include("Mission")
                .Include("EventParticipants");
        }

        public override async Task<IQueryable<Event>> GetAllAsync()
        {
            var all = await base.DbSet
                .OrderByDescending(e => e.CreatedOn)
                .Include("Platform")
                .Include("Organizer")
                .Include("Mission")
                .Include("EventParticipants")
                .ToListAsync();
            return all.AsQueryable();
        }

        public override Event Find(int id)
        {
            return this.DbSet
                .Where(c => c.Id == id)
                .Include("Platform")
                .Include("Organizer")
                .Include("Mission")
                .Include("EventParticipants")
                .FirstOrDefault();
        }

        public override async Task<Event> FindAsync(int id)
        {
            return await this.DbSet
                .Where(c => c.Id == id)
                .Include("Platform")
                .Include("Organizer")
                .Include("Mission")
                .Include("EventParticipants")
                .FirstOrDefaultAsync();
        }

        public override Event Find(int? id)
        {
            return this.DbSet
                .Where(c => c.Id == Convert.ToInt32(id))
                .Include("Platform")
                .Include("Organizer")
                .Include("Mission")
                .Include("EventParticipants")
                .FirstOrDefault();
        }

        public async Task<IQueryable<Event>> GetAllByMission(int missionId)
        {
            var all = await this.GetAllAsync();
            return all.Where(e => e.Mission.Id == missionId);
        }

        public IEnumerable<Event> GetRecentlyHostedEvents(string userId, int count = 3)
        {
            return this.DbSet
                .Include("Organizer")
                .Where(e => e.Organizer.Id == userId)
                .OrderByDescending(e => e.CreatedOn)
                .Take(count);
        }

        public IEnumerable<Event> GetRecentlyJoinedEvents(string userId, int count = 3)
        {
            return this.GetAll()
                .Where(e => e.EventParticipants.Any(ep => ep.User.Id == userId && e.Organizer.Id != userId))
                .OrderByDescending(e => e.CreatedOn)
                .Take(count);
        }
    }
}
