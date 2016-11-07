namespace Eventing.Common
{
    public interface IEventStore
    {
        void Add(object e);
    }
}