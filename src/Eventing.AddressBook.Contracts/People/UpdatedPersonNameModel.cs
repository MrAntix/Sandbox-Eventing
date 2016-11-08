using System;

namespace Eventing.AddressBook.Contracts.People
{
    public class UpdatedPersonNameModel
    {
        public UpdatedPersonNameModel(Guid identifier, string newName)
        {
            Identifier = identifier;
            NewName = newName;
        }

        public Guid Identifier { get; set; }
        public string NewName { get; }
    }
}