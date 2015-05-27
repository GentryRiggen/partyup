using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models.DTO
{
    public class UserDTO : IDto<User>
    {
        public String Id { get; set; }
        public String UserName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }

        public UserDTO(User u)
        {
            this.Id = u.Id;
            this.UserName = u.UserName;
            this.FirstName = u.FirstName;
            this.LastName = u.LastName;
            this.Email = u.Email;
        }

        public User ToModel()
        {
            return new User() 
            {
                Id = this.Id,
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email
            };
        }
    }
}
