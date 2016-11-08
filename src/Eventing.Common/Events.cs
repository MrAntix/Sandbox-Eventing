using System;

namespace Eventing.Common
{
    public class Events : IEvents
    {
        readonly IEventStore _store;
        readonly IEventDispatcherRepository _dispatchers;

        public Events(IEventStore store, IEventDispatcherRepository dispatchers)
        {
            _store = store;
            _dispatchers = dispatchers;
        }

        void IEvents.Raise(object data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var type = data.GetType();
            IEventDispatcher dispatcher;

            if (!_dispatchers.TryGetDispatcher(type, out dispatcher)) return;

            var e = dispatcher.Wrap(data);
            _store.Add(e);

            dispatcher.Dispatch(e);
        }
    }
}