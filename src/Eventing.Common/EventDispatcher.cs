using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventing.Common
{
    public class EventDispatcher : IEventDispatcher
    {
        readonly Func<object, IEvent> _wrap;
        readonly IList<Action<IEvent>> _handlers;

        public EventDispatcher(Func<object, IEvent> wrap)
        {
            _wrap = wrap;
            _handlers = new List<Action<IEvent>>();
        }

        public void AddHandler<T>(IEventHandler<T> handler)
        {
            _handlers.Add(e => handler.Handle((Event<T>) e));
        }

        IEvent IEventDispatcher.Wrap(object data)
        {
            return _wrap(data);
        }

        public void Dispatch(IEvent e)
        {
            foreach (var handler in _handlers.ToArray())
            {
                handler(e);
            }
        }
    }
}