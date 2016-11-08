using System;
using Eventing.AddressBook.Contracts.People;
using Eventing.Common;

namespace Eventing.AddressBook.Domain.People
{
    public class CreatePersonCommandHandler :
        ICommandHandlerInOut<CreatePersonCommand, CreatedPersonModel>
    {
        readonly IEvents _events;
        readonly CreatePersonFriendCommandHandler _createFriend;

        public CreatePersonCommandHandler(
            IEvents events,
            CreatePersonFriendCommandHandler createFriend)
        {
            _events = events;
            _createFriend = createFriend;
        }

        public CreatedPersonModel Execute(CreatePersonCommand command)
        {
            var person = new CreatedPersonModel(Guid.NewGuid(), command.Name);

            _events.Raise(person);

            var friend = _createFriend
                .Execute(new CreatePersonFriendCommand(person.Identifier));
            person.FriendIdentifier = friend.Identifier;

            return person;
        }
    }
}