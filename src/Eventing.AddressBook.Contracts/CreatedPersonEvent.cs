namespace Eventing.AddressBook.Contracts
{
    public class CreatedPersonEvent
    {
        public CreatedPersonEvent(CreatedPersonModel createdPerson)
        {
            CreatedPerson = createdPerson;
        }

        public CreatedPersonModel CreatedPerson { get; }
    }
}