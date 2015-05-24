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

        public void Dispose()
        {
            if (this._context != null)
            {
                this._context.Dispose();
            }
        }
    }
}
