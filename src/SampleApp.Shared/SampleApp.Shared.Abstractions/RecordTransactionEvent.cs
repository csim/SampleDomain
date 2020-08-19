namespace SampleApp.Shared.Abstractions
{
    using System;
    using NServiceBus;

    public class RecordTransactionEvent<T> : IEvent where T : RecordBase
    {
        public T Record { get; set; }

        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}
