namespace Sample.Domain.Records
{
    using Sample.Domain.Events;
    using Sample.Shared.Abstractions;

    public class ToDoItemRecord : RecordBase
    {
        public string Description { get; set; }

        public bool IsDone { get; private set; }

        public string Title { get; set; } = string.Empty;

        public void MarkComplete()
        {
            IsDone = true;

            Events.Add(new ToDoItemCompletedEvent(this));
        }

        public override string ToString()
        {
            string status = IsDone ? "Done!" : "Not done.";
            return $"{Id}: Status: {status} - {Title} - {Description}";
        }
    }
}
