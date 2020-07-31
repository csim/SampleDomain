namespace Sample.Domain.Events
{
    using Sample.Domain.Records;
    using Sample.Shared.Abstractions;

    public class ToDoItemCompletedEvent : EventBase
    {
        public ToDoItemCompletedEvent(ToDoItemRecord completedItem)
        {
            CompletedItem = completedItem;
        }

        public ToDoItemRecord CompletedItem { get; set; }
    }
}
