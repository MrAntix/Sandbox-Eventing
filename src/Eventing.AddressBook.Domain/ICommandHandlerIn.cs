namespace Eventing.AddressBook.Domain
{
    public interface ICommandHandlerIn<in TIn>
    {
        void Execute(TIn command);
    }
}