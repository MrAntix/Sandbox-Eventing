namespace Eventing.AddressBook.Domain.People
{
    public class CreatePersonCommand
    {
        public CreatePersonCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}