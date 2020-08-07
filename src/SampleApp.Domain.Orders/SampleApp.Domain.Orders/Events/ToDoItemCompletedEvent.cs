namespace SampleApp.Domain.Orders.Events
{
    using SampleApp.Domain.Orders.Records;
    using SampleApp.Shared.Abstractions;

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
