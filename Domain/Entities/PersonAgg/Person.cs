using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PersonAgg
{
    public class Person
    {
        public List<string> AddressLines { get; set; } = new List<string>();
    }
}
