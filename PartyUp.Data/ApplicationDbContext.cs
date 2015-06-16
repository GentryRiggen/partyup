using Microsoft.AspNet.Identity.EntityFramework;
using PartyUp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Community> Communities { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }

        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
            // Log the SQL statements to the console
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
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
            try
            {
                this.ApplyRules();
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                    ); // Add the original exception as the innerException
            }
            
        }
    }
}
