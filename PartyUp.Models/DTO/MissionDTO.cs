using System;
using System.Collections.Generic;

namespace PartyUp.Models.DTO
{
    public class MissionDTO : IDto<Mission>
    {
        public int Id { get; set; }
        public int CommunityId { get; set; }
        public string CommunityName { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String LogoUrl { get; set; }
        public String BannerUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public MissionDTO()
        {
            // NoOp
        }

        public MissionDTO(Mission m)
        {
            this.Id = m.Id;
            this.CommunityId = m.Community.Id;
            this.CommunityName = m.Community.Name;
            this.Name = m.Name;
            this.Description = m.Description;
            this.LogoUrl = m.LogoUrl;
            this.BannerUrl = m.BannerUrl;
            this.CreatedOn = m.CreatedOn;
            this.ModifiedOn = m.ModifiedOn;
        }

        public Mission ToModel()
        {
            return new Mission() 
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

        public Mission UpdateDbModel(Mission entity)
        {
            entity.Name = this.Name;
            entity.Description = this.Description;
            entity.BannerUrl = this.BannerUrl;

            return entity;
        }
    }
}
