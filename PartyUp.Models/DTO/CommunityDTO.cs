using System;
using System.Collections.Generic;

namespace PartyUp.Models.DTO
{
    public class CommunityDTO : IDto<Community>
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String LogoUrl { get; set; }
        public String BannerUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public CommunityDTO()
        {
            // NoOp
        }

        public CommunityDTO(Community c)
        {
            this.Id = c.Id;
            this.Name = c.Name;
            this.Description = c.Description;
            this.LogoUrl = c.LogoUrl;
            this.BannerUrl = c.BannerUrl;
            this.CreatedOn = c.CreatedOn;
            this.ModifiedOn = c.ModifiedOn;
        }

        public Community ToModel()
        {
            return new Community() 
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                LogoUrl = this.LogoUrl,
                BannerUrl = this.BannerUrl,
                CreatedOn = this.CreatedOn,
                ModifiedOn = this.ModifiedOn
            };
        }
    }
}
