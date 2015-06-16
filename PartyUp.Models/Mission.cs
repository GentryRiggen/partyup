using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models
{
    public class Mission : IAutoDates
    {
        public int Id { get; set; }

        [Required]
        public virtual Community Community { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public String LogoUrl { get; set; }

        public String BannerUrl { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
