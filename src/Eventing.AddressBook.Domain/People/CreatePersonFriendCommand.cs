using System;

namespace Eventing.AddressBook.Domain.People
{
    public class CreatePersonFriendCommand
    {
        public CreatePersonFriendCommand(Guid friendIdentifier)
        {
            FriendIdentifier = friendIdentifier;
        }

        public Guid FriendIdentifier { get; }
    }
}