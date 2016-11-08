using System;

namespace Eventing.Common
{
    public interface IEvent
    {
        long SequenceNumber { get; }
        DateTimeOffset On { get; }
        object Data { get; }
    }
}