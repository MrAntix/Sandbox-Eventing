namespace Eventing.Common
{
    public interface IEvents
    {
        void Register<T>(IEventHandler<T> handler);
        void Raise(object data);
        void Replay(IEvent e);
    }
}