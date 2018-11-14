using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueLib.Tests.Helper
{
    class PersonOverridedEquals
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PersonOverridedEquals(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            PersonOverridedEquals person = other as PersonOverridedEquals;
            if (person.FirstName == FirstName && person.LastName == LastName)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return FirstName.GetHashCode()+LastName.GetHashCode();
        }
    }
}
