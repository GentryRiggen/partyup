using Microsoft.AspNet.Identity.EntityFramework;
using PartyUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

    }
}
