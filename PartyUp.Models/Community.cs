﻿using System;
using System.Collections.Generic;

namespace PartyUp.Models
{
    public class Community : IAutoDates
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public virtual ICollection<Platform> SupportedPlatforms { get; set; }

        public String LogoUrl { get; set; }

        public String BannerUrl { get; set; }

        public virtual ICollection<Mission> Missions { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
