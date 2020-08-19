namespace SampleApp.Orders.Client.Records
{
    using System;
    using System.Collections.Generic;
    using NServiceBus;
    using SampleApp.Orders.Client.Events;
    using SampleApp.Shared.Abstractions;

    public class OrderRecord : RecordBase
    {
        public ICollection<OrderItemRecord> Items { get; set; } = new List<OrderItemRecord>();

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
}
