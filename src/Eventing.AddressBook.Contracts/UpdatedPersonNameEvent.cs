using System;

namespace Eventing.AddressBook.Contracts
{
    public class UpdatedPersonNameEvent
    {
        public UpdatedPersonNameEvent(UpdatedPersonNameModel updatedPerson)
        {
            UpdatedPerson = updatedPerson;
        }

        public UpdatedPersonNameModel UpdatedPerson { get; }
    }
}