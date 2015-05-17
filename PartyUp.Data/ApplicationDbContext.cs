using Microsoft.AspNet.Identity.EntityFramework;
using PartyUp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        private void ApplyRules()
        {
            foreach (var entry in this.ChangeTracker.Entries().Where(
                        e => e.Entity is IAutoDates || e.Entity is User &&
                        ((e.State == EntityState.Added) || (e.State == EntityState.Modified))
                    ))
            {
                if (entry.Entity is IAutoDates)
                {
                    IAutoDates e = (IAutoDates)entry.Entity;
                    if (entry.State == EntityState.Added)
                        e.CreatedOn = DateTime.Now;

                    e.ModifiedOn = DateTime.Now;
                }
                else if (entry.Entity is User)
                {
                    User u = (User)entry.Entity;
                    var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    var random = new Random();
                    u.SecuritySecret = new string(
                        Enumerable.Repeat(chars, 12)
                                  .Select(s => s[random.Next(s.Length)])
                                  .ToArray());                    
                }
            }
        }

        public override int SaveChanges()
        {
            this.ApplyRules();
            return base.SaveChanges();
        }
    }
}
