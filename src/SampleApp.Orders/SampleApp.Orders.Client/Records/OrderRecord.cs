namespace SampleApp.Orders.Client.Records
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using NServiceBus;
    using SampleApp.Orders.Client.Events;
    using SampleApp.Shared.Abstractions;

    public class OrderRecord : RecordBase
    {
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public long Number { get; set; }

        public decimal Total { get; set; }

        public override IMessage[] AddedMessages()
        {
            return new IMessage[] { new OrderRecordAuditCommand { Record = this, TransactionType = RecordTransactionType.Add } };
        }

        public override IMessage[] DeletedMessages()
        {
            return new IMessage[] { new OrderRecordAuditCommand { Record = this, TransactionType = RecordTransactionType.Delete } };
        }

        public override IMessage[] UpdatedMessages()
        {
            return new IMessage[] { new OrderRecordAuditCommand { Record = this, TransactionType = RecordTransactionType.Update } };
        }
    }

    [Owned]
    public class OrderItem
    {
        public decimal Price { get; set; }

        public CompactProductRecord Product { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
