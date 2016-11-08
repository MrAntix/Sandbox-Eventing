using Eventing.AddressBook.Contracts.People;
using Eventing.Common;

namespace Eventing.AddressBook.Domain.People
{
    public class UpdatePersonNameCommandHandler :
        ICommandHandlerIn<UpdatePersonNameCommand>
    {
        readonly IEvents _events;

        public UpdatePersonNameCommandHandler(
            IEvents events)
        {
            _events = events;
        }

        public void Execute(UpdatePersonNameCommand command)
        {
            _events.Raise(
                new UpdatedPersonNameModel(command.Identifier, command.NewName)
            );
        }
    }
}