using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessingService.Domain.Models
{
    public class Person
    {
        public string FullName { get; set; }

        public List<PersonProperty> Properties { get; set; }

        public List<Position> Positions { get; set; }
    }
}
