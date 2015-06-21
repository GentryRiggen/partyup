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
        public string XBLTag { get; set; }
        public string PSNTag { get; set; }
        public string SteamTag { get; set; }

        public UserDTO(User u)
        {
            this.Id = u.Id;
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
                Id = this.Id,
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
