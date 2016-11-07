using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventing.AddressBook.Application
{
    public class PersonReader
    {
        readonly IEnumerable<Person> _people;

        public PersonReader(IEnumerable<Person> people)
        {
            _people = people;
        }

        public Person Read(Guid identifier)
        {
            return _people.Single(p => p.Identifier == identifier);
        }
    }
}