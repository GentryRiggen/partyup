namespace PartyUp.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PartyUp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<PartyUp.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PartyUp.Data.ApplicationDbContext context)
        {
            #region USER AND ROLES
            var UserManager = new UserManager<User>(
                new UserStore<User>(context));
            var RoleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            string adminUser = "admin";
            string basicUser = "basic";
            string proUser = "pro";

            string initialPassword = "partyup";
            string adminRole = "Admin";
            string basicRole = "Basic";
            string proRole = "Pro";
            string moderatorRole = "Moderator";

            if (!RoleManager.RoleExists(adminRole)) { RoleManager.Create(new IdentityRole(adminRole)); }
            if (!RoleManager.RoleExists(basicRole)) { RoleManager.Create(new IdentityRole(basicRole)); }
            if (!RoleManager.RoleExists(proRole)) { RoleManager.Create(new IdentityRole(proRole)); }
            if (!RoleManager.RoleExists(moderatorRole)) { RoleManager.Create(new IdentityRole(moderatorRole)); }

            if (UserManager.FindByName("gentryriggen") == null)
            {
                User g = new User()
                {
                    UserName = "g",
                    FirstName = "Gentry",
                    LastName = "Riggen",
                    Email = "gentry@partyup.io",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                };
                var result = UserManager.Create(g, initialPassword);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(g.Id, adminRole);
                    UserManager.AddToRole(g.Id, moderatorRole);
                }
            }

            if (UserManager.FindByName("mattslaughter") == null)
            {
                User m = new User()
                {
                    UserName = "m",
                    FirstName = "Matt",
                    LastName = "Slaughter",
                    Email = "matt@partyup.io",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                };
                var result = UserManager.Create(m, initialPassword);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(m.Id, basicRole);
                }
            }

            if (UserManager.FindByName(adminUser) == null)
            {
                User admin = new User()
                {
                    UserName = adminUser,
                    FirstName = adminUser,
                    LastName = "partyup",
                    Email = adminUser + "@partyup.io",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                };
                var result = UserManager.Create(admin, initialPassword);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(admin.Id, adminRole);
                }
            }

            if (UserManager.FindByName(basicUser) == null)
            {
                User basic = new User()
                {
                    UserName = basicUser,
                    FirstName = basicUser,
                    LastName = "partyup",
                    Email = basicUser + "@partyup.io",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                };
                var result = UserManager.Create(basic, initialPassword);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(basic.Id, basicRole);
                }
            }

            if (UserManager.FindByName(proUser) == null)
            {
                User pro = new User()
                {
                    UserName = proUser,
                    FirstName = proUser,
                    LastName = "partyup",
                    Email = proUser + "@partyup.io",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                };
                var result = UserManager.Create(pro, initialPassword);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(pro.Id, proRole);
                }
            }
            #endregion

            DateTime dateNow = DateTime.UtcNow;
            #region COMMUNITIES
            String xoDestinyName = "Destiny (Xbox One)";
            String x3DestinyName = "Destiny (Xbox 360)";
            String ps4DestinyName = "Destiny (Playstation 4)";
            String ps3DestinyName = "Destiny (Playstation 3)";
            context.Communities.AddOrUpdate(
              c => c.Name,
              new Community 
              { 
                  Name = "Destiny (Xbox One)",
                  Description = "Destiny by Bungie/Activtion for Xbox One",
                  CreatedOn = dateNow,
                  ModifiedOn = dateNow
              },
              new Community
              {
                  Name = "Destiny (Xbox 360)",
                  Description = "Destiny by Bungie/Activtion for Xbox 360",
                  CreatedOn = dateNow,
                  ModifiedOn = dateNow
              },
              new Community
              {
                  Name = "Destiny (Playstation 4)",
                  Description = "Destiny by Bungie/Activtion for Playstation 4",
                  CreatedOn = dateNow,
                  ModifiedOn = dateNow
              },
              new Community
              {
                  Name = "Destiny (Playstation 3)",
                  Description = "Destiny by Bungie/Activtion for Playstation 3",
                  CreatedOn = dateNow,
                  ModifiedOn = dateNow
              }
            );

            Community destinyOnXboxOne = context.Communities.Where(c => c.Name == xoDestinyName).FirstOrDefault();
            Community destinyOnXbox360 = context.Communities.Where(c => c.Name == x3DestinyName).FirstOrDefault();
            Community destinyOnPs4 = context.Communities.Where(c => c.Name == ps4DestinyName).FirstOrDefault();
            Community destinyOnPs3 = context.Communities.Where(c => c.Name == ps3DestinyName).FirstOrDefault();
            #endregion COMMUNITIES

            #region MISSIONS
            context.Missions.AddOrUpdate(
              m => m.Description,
              new Mission
              {
                  Name = "Vault of Glass (Normal)",
                  Description = "Vault of Glass RAID on Normal Mode. Xbox One",
                  Community = destinyOnXboxOne,
                  CreatedOn = dateNow,
                  ModifiedOn = dateNow
              },
              new Mission
              {
                  Name = "Vault of Glass (Normal)",
                  Description = "Vault of Glass RAID on Normal Mode. Xbox 360",
                  Community = destinyOnXbox360,
                  CreatedOn = dateNow,
                  ModifiedOn = dateNow
              },
              new Mission
              {
                  Name = "Vault of Glass (Normal)",
                  Description = "Vault of Glass RAID on Normal Mode. PS4",
                  Community = destinyOnPs4,
                  CreatedOn = dateNow,
                  ModifiedOn = dateNow
              },
              new Mission
              {
                  Name = "Vault of Glass (Normal)",
                  Description = "Vault of Glass RAID on Normal Mode. PS3",
                  Community = destinyOnPs3,
                  CreatedOn = dateNow,
                  ModifiedOn = dateNow
              }
            );
            #endregion
        }
    }
}
