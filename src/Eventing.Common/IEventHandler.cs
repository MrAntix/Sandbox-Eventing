namespace Eventing.Common
{
    public interface IEventHandler<in T>
    {
        void Handle(T e);
    }
}