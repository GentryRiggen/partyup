using PartyUp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data
{
    public class ApplicationDataFactory : IDisposable
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private UserRepository _users = null;
        public UserRepository Users
        {
            get
            {
                if (this._users == null)
                {
                    this._users = new UserRepository(this._context);
                }
                return this._users;
            }
        }

        private CommunityRepository _communities = null;
        public CommunityRepository Communities
        {
            get
            {
                if (this._communities == null)
                {
                    this._communities = new CommunityRepository(this._context);
                }
                return this._communities;
            }
        }

        private MissionRepository _missions = null;
        public MissionRepository Missions
        {
            get
            {
                if (this._missions == null)
                {
                    this._missions = new MissionRepository(this._context);
                }
                return this._missions;
            }
        }

        private EventRepository _events = null;
        public EventRepository Events
        {
            get
            {
                if (this._events == null)
                {
                    this._events = new EventRepository(this._context);
                }
                return this._events;
            }
        }

        private EventParticipantRepository _eventParticipants = null;
        public EventParticipantRepository EventParticipants
        {
            get
            {
                if (this._eventParticipants == null)
                {
                    this._eventParticipants = new EventParticipantRepository(this._context);
                }
                return this._eventParticipants;
            }
        }

        public ApplicationDbContext Context
        {
            get
            {
                return _context;
            }
        }

        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (this._context != null)
            {
                this._context.Dispose();
            }
        }
    }
}
