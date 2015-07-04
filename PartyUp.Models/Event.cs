using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public int DesiredAmount { get; set; }

        public string Notes { get; set; }

        public bool HasMic { get; set; }

        public string Language { get; set; }

        public virtual ICollection<User> EventParticipants { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
