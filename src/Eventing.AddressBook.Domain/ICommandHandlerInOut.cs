namespace Eventing.AddressBook.Domain
{
    public interface ICommandHandlerInOut<in TIn, out TOut>
    {
        TOut Execute(TIn command);
    }
}