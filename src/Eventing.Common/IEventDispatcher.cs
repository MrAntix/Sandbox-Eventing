namespace Eventing.Common
{
    public interface IEventDispatcher
    {
        IEvent Wrap(object data);
        void Dispatch(IEvent e);
    }
}