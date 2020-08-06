namespace Sample.Ordering.Domain.Records
{
    using System;
    using Sample.Abstractions;

    public class OrderItemRecord : RecordBase, IRecordCompactible
    {
        public Guid ProductId { get; set; }

        public ProductCompact Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public object ToCompact()
        {
            return ToCompact<OrderItemRecord>();
        }

        public T ToCompact<T>() where T : class, new()
        {
            return SampleOrderingDomainModule.Mapper.Map<T>(this);
        }
    }

    public class CompactRecordBase
    {

    }

    public class OrderItemCompact : CompactRecordBase
    {
        public Guid? Id { get; set; }
        
        public Guid? ProductId { get; set; }

        public string ProductName { get; set; }
    }
}
