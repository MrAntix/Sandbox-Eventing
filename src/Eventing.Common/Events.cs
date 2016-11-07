using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Eventing.Common
{
    public class Events : IEvents
    {
        readonly IEventStore _store;

        readonly IDictionary<Type, IList> _handlers
            = new ConcurrentDictionary<Type, IList>();

        public Events(IEventStore store)
        {
            _store = store;
        }

        void IEvents.Register<T>(IEventHandler<T> handler)
        {
            var type = typeof(T);
            IList registry;

            if (!_handlers.TryGetValue(type, out registry))
            {
                registry = new List<IEventHandler<T>>();
                _handlers.Add(type, registry);
            }

            _handlers[type].Add(handler);
        }

        void IEvents.Raise<T>(T e)
        {
            _store.Add(e);

            RaiseInternal(e);
        }

        void RaiseInternal<T>(T e)
        {
            var type = e.GetType();
            IList registry;

            if (!_handlers.TryGetValue(type, out registry)) return;

            foreach (var handler in registry.Cast<IEventHandler<T>>().ToArray())
            {
                handler.Handle(e);
            }
        }

        void IEvents.Replay(object e)
        {
            var method = typeof(Events)
                .GetMethod("RaiseInternal", BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(e.GetType());

            method.Invoke(this, new[] {e});
        }
    }
}