using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Eventing.Common
{
    public class EventDispatcherRepository : IEventDispatcherRepository
    {
        readonly Func<long> _getNextSequenceNumber;

        readonly IDictionary<Type, IEventDispatcher> _dispatchers
            = new ConcurrentDictionary<Type, IEventDispatcher>();

        public EventDispatcherRepository(Func<long> getNextSequenceNumber)
        {
            _getNextSequenceNumber = getNextSequenceNumber;
        }

        bool IEventDispatcherRepository.TryGetDispatcher(Type type, out IEventDispatcher dispatcher)
        {
            return _dispatchers.TryGetValue(type, out dispatcher);
        }

        void IEventDispatcherRepository.Register<T>(IEventHandler<T> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            var type = typeof(T);
            IEventDispatcher dispatcher;

            if (!_dispatchers.TryGetValue(type, out dispatcher))
            {
                dispatcher = new EventDispatcher(
                    data => new Event<T>(
                        _getNextSequenceNumber(), DateTimeOffset.UtcNow,
                        (T) data));

                _dispatchers.Add(type, dispatcher);
            }

            ((EventDispatcher) dispatcher).AddHandler(handler);
        }
    }
}