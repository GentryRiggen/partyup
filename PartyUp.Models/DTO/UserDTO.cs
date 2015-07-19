using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models.DTO
{
    public class UserDTO : IDto<User>
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("username")]
        public String UserName { get; set; }

        [JsonProperty("firstName")]
        public String FirstName { get; set; }

        [JsonProperty("lastName")]
        public String LastName { get; set; }

        [JsonProperty("email")]
        public String Email { get; set; }

        [JsonProperty("xblTag")]
        public string XBLTag { get; set; }

        [JsonProperty("psnTag")]
        public string PSNTag { get; set; }

        [JsonProperty("steamTag")]
        public string SteamTag { get; set; }

        public UserDTO()
        {
            // noop
        }

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
