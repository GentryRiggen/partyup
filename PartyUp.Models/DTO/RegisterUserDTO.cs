using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models.DTO
{
    public class RegisterUserDTO : IDto<User>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string XBLTag { get; set; }
        public string PSNTag { get; set; }
        public string SteamTag { get; set; }
        public string Password { get; set; }

        public RegisterUserDTO()
        {
            //Noop
        }

        public RegisterUserDTO(User u)
        {
            this.UserName = u.UserName;
            this.FirstName = u.FirstName;
            this.LastName = u.LastName;
            this.Email = u.Email;
            this.XBLTag = u.XBLTag;
            this.PSNTag = u.PSNTag;
            this.SteamTag = u.SteamTag;
        }

        public User ToModel()
        {
            return new User() 
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                XBLTag = this.XBLTag,
                PSNTag = this.PSNTag,
                SteamTag = this.SteamTag
            };
        }

        public User UpdateDbModel(User entity)
        {
            entity.UserName = this.UserName;
            entity.FirstName = this.FirstName;
            entity.LastName = this.LastName;
            entity.Email = this.Email;
            entity.XBLTag = this.XBLTag;
            entity.PSNTag = this.PSNTag;
            entity.SteamTag = this.SteamTag;

            return entity;
        }
    }
}
