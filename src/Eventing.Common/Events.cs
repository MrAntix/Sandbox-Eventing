using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Eventing.Common
{
    public class Events : IEvents
    {
        readonly IEventStore _store;

        readonly IDictionary<Type, Func<object, IEvent>> _wrappers
            = new ConcurrentDictionary<Type, Func<object, IEvent>>();

        readonly IDictionary<Type, IList<Action<IEvent>>> _handlers
            = new ConcurrentDictionary<Type, IList<Action<IEvent>>>();

        public Events(IEventStore store)
        {
            _store = store;
        }

        void IEvents.Register<T>(IEventHandler<T> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            var type = typeof(T);
            IList<Action<IEvent>> registry;

            if (!_handlers.TryGetValue(type, out registry))
            {
                registry = new List<Action<IEvent>>();
                _handlers.Add(type, registry);

                _wrappers.Add(type, data => new Event<T>(
                        _store.NextSequenceNumber, DateTimeOffset.UtcNow,
                        (T) data)
                );
            }

            _handlers[type].Add(d => handler.Handle((Event<T>) d));
        }

        void IEvents.Raise(object data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var type = data.GetType();
            Func<object, IEvent> wrapper;

            if (!_wrappers.TryGetValue(type, out wrapper)) return;

            var e = wrapper(data);

            Raise(e);
        }

        void IEvents.Replay(IEvent e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            Raise(e);
        }

        void Raise(IEvent e)
        {
            var type = e.Data.GetType();
            IList<Action<IEvent>> registry;

            if (!_handlers.TryGetValue(type, out registry)) return;

            _store.Add(e);

            foreach (var handler in registry.ToArray())
            {
                handler(e);
            }
        }
    }
}