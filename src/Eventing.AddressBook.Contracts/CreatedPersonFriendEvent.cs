using System;

namespace Eventing.AddressBook.Contracts
{
    public class CreatedPersonFriendEvent
    {
        public CreatedPersonFriendEvent(
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