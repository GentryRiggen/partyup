using PartyUp.Models;
using PartyUp.Models.DTO;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public User GetById(string id)
        {
            return base.DbSet.Where(u => u.Id.Equals(id)).FirstOrDefault();
        }

        public async Task<UserDTO> GetDTOById(string id) {
            return await base.DbSet.Select(u => 
                new UserDTO()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }).SingleOrDefaultAsync(u => u.Id.Equals(id));
        }

        public User FindByToken(string token)
        {
            return null;
        }

    }
}
