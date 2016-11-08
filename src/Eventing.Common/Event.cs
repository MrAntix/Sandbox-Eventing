using System;

namespace Eventing.Common
{
    public class Event<T> : IEvent
    {
        public Event(
            long sequenceNumber, DateTimeOffset on,
            T data)
        {
            SequenceNumber = sequenceNumber;
            On = on;
            Data = data;
        }

        public long SequenceNumber { get; }
        public DateTimeOffset On { get; }
        public T Data { get; }

        object IEvent.Data => Data;
    }
}