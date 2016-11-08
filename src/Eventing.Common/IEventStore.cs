namespace Eventing.Common
{
    public interface IEventStore
    {
        long NextSequenceNumber { get; }
        long Add(IEvent e);
        IEvent Get(long sequenceNumber);
    }
}