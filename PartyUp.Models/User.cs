using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models
{
    public class User : IdentityUser, IAutoDates
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String SecuritySecret { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<Event> HostedEvents { get; set; }

        public string XBLTag { get; set; }
        public string PSNTag { get; set; }
        public string SteamTag { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
