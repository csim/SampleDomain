namespace SampleApp.Orders.Client.Events
{
    using System;
    using NServiceBus;

    public class OrderSubmittedEvent : IEvent
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
