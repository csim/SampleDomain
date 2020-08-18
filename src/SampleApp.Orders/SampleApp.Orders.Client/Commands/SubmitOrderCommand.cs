namespace SampleApp.Orders.Client.Commands
{
    using System;
    using NServiceBus;

    public class SubmitOrderCommand : ICommand
    {
        public long Number { get; set; }
    }

    public class SubmitOrderResponse : IMessage
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
