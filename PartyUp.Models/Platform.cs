using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models
{
    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Community> Communities { get; set; }
    }
}
