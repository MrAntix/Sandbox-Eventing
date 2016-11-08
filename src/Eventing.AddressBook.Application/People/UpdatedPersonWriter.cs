using System.Collections.Generic;
using System.Linq;
using Eventing.AddressBook.Contracts.People;

namespace Eventing.AddressBook.Application.People
{
    public class UpdatedPersonWriter
    {
        readonly IEnumerable<Person> _people;

        public UpdatedPersonWriter(IEnumerable<Person> people)
        {
            _people = people;
        }

        public void Write(UpdatedPersonNameModel updated)
        {
            var person = _people.Single(p => p.Identifier == updated.Identifier);
            person.Name = updated.NewName;
        }
    }
}