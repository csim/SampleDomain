namespace SampleApp.Orders.Client.Events
{
    using SampleApp.Orders.Client.Records;
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
            return new ToDoItemCompletedEvent { Record = record };
        }
    }
}
