using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models
{
    public class Event : IAutoDates
    {
        public int Id { get; set; }

        public virtual Mission Mission { get; set; }

        public virtual User Organizer { get; set; }

        public virtual ICollection<EventParticipant> EventParticipants { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
