namespace PartyUp.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PartyUp.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<PartyUp.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PartyUp.Data.ApplicationDbContext context)
        {
            // Just so it doesn't accidentally run
            //if (true) return;

            try
            {
                #region USER AND ROLES
                var UserManager = new UserManager<User>(
                    new UserStore<User>(context));
                var RoleManager = new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));

                string initialPassword = "partyup";
                // ROLES
                string adminRole = "Admin";
                string moderatorRole = "Moderator";
                if (!RoleManager.RoleExists(adminRole)) { RoleManager.Create(new IdentityRole(adminRole)); }
                if (!RoleManager.RoleExists(moderatorRole)) { RoleManager.Create(new IdentityRole(moderatorRole)); }

                // USER CREATION
                if (UserManager.FindByName("g") == null)
                {
                    User g = new User()
                    {
                        UserName = "g",
                        FirstName = "Gentry",
                        LastName = "Riggen",
                        Email = "gentry@partyup.io",
                        XBLTag = "ST3ALTHY PANDA",
                        PSNTag = "ST3ALTHY PANDA",
                        SteamTag = "ST3ALTHY PANDA",
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
                #endregion

                #region PLATFORMS
                Platform xbox360 = new Platform { Name = "Xbox 360" };
                Platform xboxOne = new Platform { Name = "Xbox One" };
                Platform ps3 = new Platform { Name = "Playstation 3" };
                Platform ps4 = new Platform { Name = "Playstation 4" };
                Platform pc = new Platform { Name = "PC" };

                context.Platforms.AddOrUpdate(p => p.Name, xbox360, xboxOne, ps3, ps4, pc);
                context.SaveChanges();
                #endregion

                DateTime dateNow = DateTime.UtcNow;
                #region COMMUNITIES
                List<Platform> destinyPlatforms = new List<Platform>();
                destinyPlatforms.Add(xbox360);
                destinyPlatforms.Add(xboxOne);
                destinyPlatforms.Add(ps3);
                destinyPlatforms.Add(ps4);

                context.Communities.AddOrUpdate(c => c.Name, new Community
                  {
                      Name = "Destiny",
                      Description = "Destiny by Bungie/Activtion",
                      BannerUrl = "http://cdn.gentryriggen.com/partyup/destinyBanner.png",
                      LogoUrl = "http://cdn.gentryriggen.com/partyup/destinyLogo.jpg",
                      SupportedPlatforms = destinyPlatforms,
                      CreatedOn = dateNow,
                      ModifiedOn = dateNow
                  });
                context.SaveChanges();
                #endregion COMMUNITIES

                #region MISSIONS
                Community destiny = context.Communities.Where(c => c.Name == "Destiny").FirstOrDefault();
                // VoG Normal
                context.Missions.Add(new Mission
                {
                    Name = "Vault of Glass (NORMAL)",
                    Description = "Vault of Glass RAID on Normal Mode.",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/VoG.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // VoG Hard
                context.Missions.Add(new Mission
                {
                    Name = "Vault of Glass (HARD)",
                    Description = "Vault of Glass RAID on Hard Mode.",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/VoG.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // PoE lvl 28
                context.Missions.Add(new Mission
                {
                    Name = "Prison of Elders (lvl 28)",
                    Description = "Prison of Elders (lvl 28)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/poe.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // PoE lvl 32
                context.Missions.Add(new Mission
                {
                    Name = "Prison of Elders (lvl 32)",
                    Description = "Prison of Elders (lvl 32)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/poe.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // PoE lvl 34
                context.Missions.Add(new Mission
                {
                    Name = "Prison of Elders (lvl 34)",
                    Description = "Prison of Elders (lvl 34)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/poe.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // PoE lvl 35
                context.Missions.Add(new Mission
                {
                    Name = "Prison of Elders (lvl 35)",
                    Description = "Prison of Elders (lvl 35)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/poe.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Crota normal
                context.Missions.Add(new Mission
                {
                    Name = "Crota's End (NORMAL)",
                    Description = "Crota's End (NORMAL)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/crota.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Crota hard
                context.Missions.Add(new Mission
                {
                    Name = "Crota's End (HARD)",
                    Description = "Crota's End (HARD)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/crota.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Trials of Osiris
                context.Missions.Add(new Mission
                {
                    Name = "Trials of Osiris",
                    Description = "Trials of Osiris",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/trialsofosiris.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Weekly Heroic Strike
                context.Missions.Add(new Mission
                {
                    Name = "Weekly Heroic Strike (30)",
                    Description = "Weekly Heroic Strike (30)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/weeklyheroicstrike.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Weekly Heroic Strike
                context.Missions.Add(new Mission
                {
                    Name = "Weekly Heroic Strike (28)",
                    Description = "Weekly Heroic Strike (28)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/weeklyheroicstrike.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Weekly Heroic Strike
                context.Missions.Add(new Mission
                {
                    Name = "Weekly Heroic Strike (24)",
                    Description = "Weekly Heroic Strike (24)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/weeklyheroicstrike.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Weekly Nightfall Strike
                context.Missions.Add(new Mission
                {
                    Name = "Weekly Nightfall Strike",
                    Description = "Weekly Nightfall Strike",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/weeklynightfallstrike.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Daily Heroic Story
                context.Missions.Add(new Mission
                {
                    Name = "Daily Heroic Story (30)",
                    Description = "Daily Heroic Story (30)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/dailyheroicstory.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Daily Heroic Story
                context.Missions.Add(new Mission
                {
                    Name = "Daily Heroic Story (28)",
                    Description = "Daily Heroic Story (28)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/dailyheroicstory.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Daily Heroic Story
                context.Missions.Add(new Mission
                {
                    Name = "Daily Heroic Story (24)",
                    Description = "Daily Heroic Story (24)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/dailyheroicstory.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Strike Playlist - Dragon (28)
                context.Missions.Add(new Mission
                {
                    Name = "Strike Playlist - Dragon (28)",
                    Description = "Strike Playlist - Dragon (28)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/strikes.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Strike Playlist - Roc (26)
                context.Missions.Add(new Mission
                {
                    Name = "Strike Playlist - Roc (26)",
                    Description = "Strike Playlist - Roc (26)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/strikes.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Strike Playlist - Tiger (24)
                context.Missions.Add(new Mission
                {
                    Name = "Strike Playlist - Tiger (24)",
                    Description = "Strike Playlist - Tiger (24)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/strikes.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Strike Playlist - Wolf (22)
                context.Missions.Add(new Mission
                {
                    Name = "Strike Playlist - Wolf (22)",
                    Description = "Strike Playlist - Wolf (22)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/strikes.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Strike Playlist - Viper (20)
                context.Missions.Add(new Mission
                {
                    Name = "Strike Playlist - Viper (20)",
                    Description = "Strike Playlist - Viper (20)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/strikes.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Strike Playlist - Eagle (18)
                context.Missions.Add(new Mission
                {
                    Name = "Strike Playlist - Eagle (18)",
                    Description = "Strike Playlist - Eagle (18)",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/strikes.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Crucible - Iron Banner
                context.Missions.Add(new Mission
                {
                    Name = "Crucible - Iron Banner",
                    Description = "Crucible - Iron Banner",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/crucible.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Crucible - Control
                context.Missions.Add(new Mission
                {
                    Name = "Crucible - Control",
                    Description = "Crucible - Control",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/crucible.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Crucible - Clash
                context.Missions.Add(new Mission
                {
                    Name = "Crucible - Clash",
                    Description = "Crucible - Clash",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/crucible.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Crucible - Rumble
                context.Missions.Add(new Mission
                {
                    Name = "Crucible - Rumble",
                    Description = "Crucible - Rumble",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/crucible.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Crucible - Salvage
                context.Missions.Add(new Mission
                {
                    Name = "Crucible - Salvage",
                    Description = "Crucible - Salvage",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/crucible.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });

                // Crucible - Combined Arms
                context.Missions.Add(new Mission
                {
                    Name = "Crucible - Combined Arms",
                    Description = "Crucible - Combined Arms",
                    Community = destiny,
                    BannerUrl = "http://cdn.gentryriggen.com/partyup/crucible.jpg",
                    CreatedOn = dateNow,
                    ModifiedOn = dateNow
                });
                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
