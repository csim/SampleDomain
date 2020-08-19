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

        public override IEvent AddedEvent()
        {
            return new OrderRecordAddedEvent { Record = this };
        }

        public override IEvent DeletedEvent()
        {
            return new OrderRecordDeletedEvent { Record = this };
        }

        public override IEvent UpdatedEvent()
        {
            return new OrderRecordUpdatedEvent { Record = this };
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
