namespace Sample.Shared
{
    using System;

    public abstract class EventBase // : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}
