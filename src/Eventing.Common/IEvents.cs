namespace Eventing.Common
{
    public interface IEvents
    {
        void Register<T>(IEventHandler<T> handler);
        void Raise<T>(T e);
        void Replay(object e);
    }
}