namespace SampleApp.Orders.Client.Events
{
    using System;
    using NServiceBus;
    using SampleApp.Orders.Client.Records;

    public class OrderRecordAuditCommand : ICommand
    {
        public OrderRecord Record { get; set; }

        public DateTime Timestamp { get; set; }

        public RecordTransactionType TransactionType { get; set; }
    }
}
