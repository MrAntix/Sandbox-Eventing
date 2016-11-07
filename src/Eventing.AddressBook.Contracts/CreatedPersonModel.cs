using System;

namespace Eventing.AddressBook.Contracts
{
    public class CreatedPersonModel
    {
        public CreatedPersonModel(Guid identifier, string name)
        {
            Identifier = identifier;
            Name = name;
        }

        public CreatedPersonModel(
            Guid identifier, string name,
            Guid friendIdentifier)
        {
            Identifier = identifier;
            Name = name;
            FriendIdentifier = friendIdentifier;
        }

        public Guid Identifier { get; set; }
        public string Name { get; }
        public Guid FriendIdentifier { get; set; }
    }
}