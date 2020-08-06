namespace Sample.Ordering.Domain.Records
{
    using System.Collections.Generic;
    using Sample.Abstractions;

    public class OrderRecord : RecordBase
    {
        public ICollection<OrderItemRecord> Items { get; set; } = new List<OrderItemRecord>();

        public string Number { get; set; }

        public decimal Total { get; set; }

        public override string ToString()
        {
            return Number;
        }
    }
}
