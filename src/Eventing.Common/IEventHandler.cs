namespace Eventing.Common
{
    public interface IEventHandler<T>
    {
        void Handle(Event<T> e);
    }
}