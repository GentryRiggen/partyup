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
            if (true) return;

            try
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

                if (UserManager.FindByName("m") == null)
                {
                    User m = new User()
                    {
                        UserName = "m",
                        FirstName = "Matt",
                        LastName = "Slaughter",
                        Email = "matt@partyup.io",
                        XBLTag = "mattscience",
                        PSNTag = "mattscience",
                        SteamTag = "mattscience",
                        CreatedOn = DateTime.UtcNow,
                        ModifiedOn = DateTime.UtcNow
                    };
                    var result = UserManager.Create(m, initialPassword);
                    if (result.Succeeded)
                    {
                        UserManager.AddToRole(m.Id, basicRole);
                    }
                }

                if (UserManager.FindByName("m") == null)
                {
                    User m = new User()
                    {
                        UserName = "k",
                        FirstName = "Kyle",
                        LastName = "Frisbie",
                        Email = "kyle@partyup.io",
                        XBLTag = "H3LLUVA HIPPO",
                        PSNTag = "H3LLUVA HIPPO",
                        SteamTag = "H3LLUVA HIPPO",
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
                      BannerUrl = "http://cdn.gentryriggen.com/partyup/destinyBanner.png",
                      CreatedOn = dateNow,
                      ModifiedOn = dateNow
                  },
                  new Community
                  {
                      Name = "Destiny (Xbox 360)",
                      Description = "Destiny by Bungie/Activtion for Xbox 360",
                      BannerUrl = "http://cdn.gentryriggen.com/partyup/destinyBanner.png",
                      CreatedOn = dateNow,
                      ModifiedOn = dateNow
                  },
                  new Community
                  {
                      Name = "Destiny (Playstation 4)",
                      Description = "Destiny by Bungie/Activtion for Playstation 4",
                      BannerUrl = "http://cdn.gentryriggen.com/partyup/destinyBanner.png",
                      CreatedOn = dateNow,
                      ModifiedOn = dateNow
                  },
                  new Community
                  {
                      Name = "Destiny (Playstation 3)",
                      Description = "Destiny by Bungie/Activtion for Playstation 3",
                      BannerUrl = "http://cdn.gentryriggen.com/partyup/destinyBanner.png",
                      CreatedOn = dateNow,
                      ModifiedOn = dateNow
                  }
                );
                context.SaveChanges();
                List<Community> destinyCommunities = new List<Community>();
                Community destinyOnXboxOne = context.Communities.Where(c => c.Name == xoDestinyName).FirstOrDefault();
                destinyCommunities.Add(destinyOnXboxOne);

                Community destinyOnXbox360 = context.Communities.Where(c => c.Name == x3DestinyName).FirstOrDefault();
                destinyCommunities.Add(destinyOnXbox360);

                Community destinyOnPs4 = context.Communities.Where(c => c.Name == ps4DestinyName).FirstOrDefault();
                destinyCommunities.Add(destinyOnPs4);

                Community destinyOnPs3 = context.Communities.Where(c => c.Name == ps3DestinyName).FirstOrDefault();
                destinyCommunities.Add(destinyOnPs3);

                #endregion COMMUNITIES

                #region MISSIONS
                // VoG Normal
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Vault of Glass (NORMAL)",
                        Description = "Vault of Glass RAID on Normal Mode.",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/VoG.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // VoG Hard
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Vault of Glass (HARD)",
                        Description = "Vault of Glass RAID on Hard Mode.",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/VoG.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // PoE lvl 28
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Prison of Elders (lvl 28)",
                        Description = "Prison of Elders (lvl 28)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/poe.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // PoE lvl 32
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Prison of Elders (lvl 32)",
                        Description = "Prison of Elders (lvl 32)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/poe.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // PoE lvl 34
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Prison of Elders (lvl 34)",
                        Description = "Prison of Elders (lvl 34)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/poe.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // PoE lvl 35
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Prison of Elders (lvl 35)",
                        Description = "Prison of Elders (lvl 35)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/poe.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Crota normal
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Crota's End (NORMAL)",
                        Description = "Crota's End (NORMAL)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/crota.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Crota hard
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Crota's End (HARD)",
                        Description = "Crota's End (HARD)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/crota.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Trials of Osiris
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Trials of Osiris",
                        Description = "Trials of Osiris",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/trialsofosiris.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Weekly Heroic Strike
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Weekly Heroic Strike (30)",
                        Description = "Weekly Heroic Strike (30)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/weeklyheroicstrike.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Weekly Heroic Strike
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Weekly Heroic Strike (28)",
                        Description = "Weekly Heroic Strike (28)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/weeklyheroicstrike.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Weekly Heroic Strike
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Weekly Heroic Strike (24)",
                        Description = "Weekly Heroic Strike (24)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/weeklyheroicstrike.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Weekly Nightfall Strike
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Weekly Nightfall Strike",
                        Description = "Weekly Nightfall Strike",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/weeklynightfallstrike.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Daily Heroic Story
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Daily Heroic Story (30)",
                        Description = "Daily Heroic Story (30)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/dailyheroicstory.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Daily Heroic Story
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Daily Heroic Story (28)",
                        Description = "Daily Heroic Story (28)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/dailyheroicstory.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Daily Heroic Story
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Daily Heroic Story (24)",
                        Description = "Daily Heroic Story (24)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/dailyheroicstory.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Strike Playlist - Dragon (28)
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Strike Playlist - Dragon (28)",
                        Description = "Strike Playlist - Dragon (28)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/strikes.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Strike Playlist - Roc (26)
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Strike Playlist - Roc (26)",
                        Description = "Strike Playlist - Roc (26)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/strikes.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Strike Playlist - Tiger (24)
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Strike Playlist - Tiger (24)",
                        Description = "Strike Playlist - Tiger (24)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/strikes.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Strike Playlist - Wolf (22)
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Strike Playlist - Wolf (22)",
                        Description = "Strike Playlist - Wolf (22)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/strikes.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Strike Playlist - Viper (20)
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Strike Playlist - Viper (20)",
                        Description = "Strike Playlist - Viper (20)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/strikes.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Strike Playlist - Eagle (18)
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Strike Playlist - Eagle (18)",
                        Description = "Strike Playlist - Eagle (18)",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/strikes.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Crucible - Iron Banner
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Crucible - Iron Banner",
                        Description = "Crucible - Iron Banner",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/crucible.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Crucible - Control
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Crucible - Control",
                        Description = "Crucible - Control",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/crucible.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Crucible - Clash
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Crucible - Clash",
                        Description = "Crucible - Clash",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/crucible.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Crucible - Rumble
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Crucible - Rumble",
                        Description = "Crucible - Rumble",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/crucible.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Crucible - Salvage
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Crucible - Salvage",
                        Description = "Crucible - Salvage",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/crucible.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }

                // Crucible - Combined Arms
                foreach (Community c in destinyCommunities)
                {
                    context.Missions.Add(new Mission
                    {
                        Name = "Crucible - Combined Arms",
                        Description = "Crucible - Combined Arms",
                        Community = c,
                        BannerUrl = "http://cdn.gentryriggen.com/partyup/crucible.jpg",
                        CreatedOn = dateNow,
                        ModifiedOn = dateNow
                    });
                }
                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
