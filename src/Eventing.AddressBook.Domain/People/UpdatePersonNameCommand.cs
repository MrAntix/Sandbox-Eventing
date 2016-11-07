using System;

namespace Eventing.AddressBook.Domain.People
{
    public class UpdatePersonNameCommand
    {
        public UpdatePersonNameCommand(
            Guid identifier,
            string newName)
        {
            Identifier = identifier;
            NewName = newName;
        }

        public string NewName { get; }
        public Guid Identifier { get; }
    }
}