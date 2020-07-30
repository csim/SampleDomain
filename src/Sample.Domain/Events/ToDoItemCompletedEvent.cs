namespace Sample.Domain.Events
{
    using Sample.Domain.Entities;
    using Sample.Shared;
    using Sample.Shared.Abstractions;

    public class ToDoItemCompletedEvent : EventBase
    {
        public ToDoItemCompletedEvent(ToDoItem completedItem)
        {
            CompletedItem = completedItem;
        }

        public ToDoItem CompletedItem { get; set; }
    }
}
