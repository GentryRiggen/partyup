using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models
{
    public class EventParticipant : IAutoDates
    {
        public int Id { get; set; }
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
