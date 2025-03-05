using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInTester
{
    public class Person
    {
        public string Name { get; set; } = "";
        public string Position { get; set; } = "";
        public bool? Validated { get; set; } = false;

        public override string ToString()
        {
            return $"{Name},{Position},{Validated}";
        }
    }

    public class Company
    {
        public string Name { get; set; } = "";
        public List<Person> People { get; set; } = new List<Person>();
    }
}
