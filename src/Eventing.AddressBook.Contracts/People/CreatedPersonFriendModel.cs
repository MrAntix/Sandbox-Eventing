using System;

namespace Eventing.AddressBook.Contracts.People
{
    public class CreatedPersonFriendModel
    {
        public CreatedPersonFriendModel(
            CreatedPersonModel createdPerson,
            Guid friendIdentifier)
        {
            CreatedPerson = createdPerson;
            FriendIdentifier = friendIdentifier;
        }

        public CreatedPersonModel CreatedPerson { get; }
        public Guid FriendIdentifier { get; }
    }
}