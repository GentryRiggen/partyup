using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models.DTO
{
    public interface IDto<T> where T : class
    {
        T toModel();
    }
}
