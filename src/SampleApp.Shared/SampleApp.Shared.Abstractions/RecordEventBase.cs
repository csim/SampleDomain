namespace SampleApp.Shared.Abstractions
{
    using System;

    public abstract class RecordEventBase<T> where T : RecordBase
    {
        public T Record { get; set; }

        public DateTime OriginatedOn { get; } = DateTime.UtcNow;
    }

    public interface IRecordEvent
    {
        public DateTime OriginatedOn { get; }
    }
}
