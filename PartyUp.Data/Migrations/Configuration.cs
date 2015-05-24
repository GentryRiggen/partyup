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
            AutomaticMigrationsEnabled = false;
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

            if (!RoleManager.RoleExists(adminRole)) { RoleManager.Create(new IdentityRole(adminRole)); }
            if (!RoleManager.RoleExists(basicRole)) { RoleManager.Create(new IdentityRole(basicRole)); }
            if (!RoleManager.RoleExists(proRole)) { RoleManager.Create(new IdentityRole(proRole)); }

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
        }
    }
}
