using System.Collections.Generic;
using Eventing.Common;

namespace Eventing.Tests
{
    public static class TestHelper
    {
        public static IEvents GetEvents(IEventStore store, IEventDispatcherRepository dispatchers)
        {
            return new Events(store, dispatchers);
        }

        public static IEventStore GetEventStore()
        {
            return new TestEventStore();
        }

        class TestEventStore : IEventStore
        {
            readonly IList<IEvent> _list;

            public TestEventStore()
            {
                _list = new List<IEvent>();
            }

            public long NextSequenceNumber => _list.Count + 1;

            public long Add(IEvent e)
            {
                _list.Add(e);

                return e.SequenceNumber;
            }

            public IEvent Get(long sequenceNumber)
            {
                return _list[(int) sequenceNumber - 1];
            }
        }
    }
}