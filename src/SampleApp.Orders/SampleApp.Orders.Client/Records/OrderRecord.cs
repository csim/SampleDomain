namespace SampleApp.Orders.Client.Records
{
    using System.Collections.Generic;
    using SampleApp.Shared.Abstractions;

    public class OrderRecord : RecordBase
    {
        public ICollection<OrderItemRecord> Items { get; set; } = new List<OrderItemRecord>();

        public long Number { get; set; }

        public decimal Total { get; set; }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
