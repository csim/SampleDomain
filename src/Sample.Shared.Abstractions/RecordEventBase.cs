namespace Sample.Abstractions
{
    using System;

    public abstract class RecordEventBase<T> where T : RecordBase
    {
        public T Record { get; set; }

        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }

    public interface IRecordEvent
    {
        public DateTime Timestamp { get; }
    }
}
