namespace Sample.Domain.Events
{
    using Sample.Abstractions;
    using Sample.Domain.Records;

    public class ToDoItemCompletedEvent : RecordEventBase<ToDoItemRecord>, IRecordEvent
    {
        private ToDoItemCompletedEvent()
        {
        }

        public ToDoItemCompletedEvent(ToDoItemRecord records)
        {
            Record = records;
        }

        public static ToDoItemCompletedEvent Create(ToDoItemRecord record)
        {
            return new ToDoItemCompletedEvent
            {
                Record = record
            };
        }
    }
}
