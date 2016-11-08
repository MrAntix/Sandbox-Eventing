namespace Eventing.Common
{
    public interface IEvents
    {
        void Raise(object data);
    }
}