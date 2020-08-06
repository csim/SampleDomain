namespace Sample.Ordering.Domain.Events
{
    using Sample.Abstractions;
    using Sample.Ordering.Domain.Records;

    public class ToDoItemCompletedEvent : RecordEventBase<OrderRecord>, IRecordEvent
    {
        private ToDoItemCompletedEvent()
        {
        }

        public ToDoItemCompletedEvent(OrderRecord records)
        {
            Record = records;
        }

        public static ToDoItemCompletedEvent Create(OrderRecord record)
        {
            return new ToDoItemCompletedEvent
            {
                Record = record
            };
        }
    }
}
