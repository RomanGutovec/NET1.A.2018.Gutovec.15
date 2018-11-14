using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueLib.Tests.Helper
{
    class PersonIEquatable: IEquatable<PersonIEquatable>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PersonIEquatable(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public bool Equals(PersonIEquatable other)
        => other.FirstName == FirstName && other.LastName == LastName;
    }
}
