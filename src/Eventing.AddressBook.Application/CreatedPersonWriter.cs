using System.Collections.Generic;
using Eventing.AddressBook.Contracts;

namespace Eventing.AddressBook.Application
{
    public class CreatedPersonWriter
    {
        readonly ICollection<Person> _people;

        public CreatedPersonWriter(ICollection<Person> people)
        {
            _people = people;
        }

        public void Write(CreatedPersonModel created)
        {
            var person = new Person
            {
                Identifier = created.Identifier,
                Name = created.Name
            };

            _people.Add(person);
        }
    }
}