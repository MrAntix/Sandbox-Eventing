using System;
using Eventing.AddressBook.Contracts;
using Eventing.Common;

namespace Eventing.AddressBook.Domain.People
{
    public class CreatePersonFriendCommandHandler :
        ICommandHandlerInOut<CreatePersonFriendCommand, CreatedPersonModel>
    {
        readonly IEvents _events;

        public CreatePersonFriendCommandHandler(
            IEvents events)
        {
            _events = events;
        }

        public CreatedPersonModel Execute(CreatePersonFriendCommand command)
        {
            var person = new CreatedPersonModel(
                Guid.NewGuid(),
                $"Friend of {command.FriendIdentifier}",
                command.FriendIdentifier);

            _events.Raise(new CreatedPersonFriendEvent(person, command.FriendIdentifier));

            return person;
        }
    }
}