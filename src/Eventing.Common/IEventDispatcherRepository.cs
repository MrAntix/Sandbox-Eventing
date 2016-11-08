using System;

namespace Eventing.Common
{
    public interface IEventDispatcherRepository
    {
        bool TryGetDispatcher(Type type, out IEventDispatcher dispatcher);
        void Register<T>(IEventHandler<T> handler);
    }
}