using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PartyUp.Models.DTO
{
    public class CommunityDTO : IDto<Community>
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("supportedPlatform")]
        public IEnumerable<string> SupportedPlatforms { get; set; }

        [JsonProperty("logoUrl")]
        public String LogoUrl { get; set; }

        [JsonProperty("bannerUrl")]
        public String BannerUrl { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("modifiedOn")]
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
            List<string> platforms = new List<string>();
            foreach (Platform platform in c.SupportedPlatforms)
            {
                platforms.Add(platform.Name);
            }
            this.SupportedPlatforms = platforms;
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

        public Community UpdateDbModel(Community entity)
        {
            entity.Name = this.Name;
            entity.Description = this.Description;
            entity.BannerUrl = this.BannerUrl;
            entity.LogoUrl = this.LogoUrl;

            return entity;
        }
    }
}
