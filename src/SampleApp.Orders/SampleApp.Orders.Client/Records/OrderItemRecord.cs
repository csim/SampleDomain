namespace SampleApp.Orders.Client.Records
{
    using System;
    using SampleApp.Shared.Abstractions;

    public class OrderItemRecord : RecordBase, ICompactableRecord
    {
        public decimal Price { get; set; }

        public CompactProductRecord Product { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public CompactRecord ToCompact()
        {
            return ToCompact<CompactOrderItemRecord>();
        }

        public T ToCompact<T>() where T : CompactRecord, new()
        {
            var targetType = typeof(T);
            object ret = null;

            if (targetType == typeof(CompactOrderItemRecord))
            {
                ret = new CompactOrderItemRecord { Id = Id, ProductId = ProductId, ProductName = Product?.Name };
            }

            if (ret != null)
            {
                return (T)Convert.ChangeType(ret, targetType);
            }

            throw new ApplicationException($"Unable to determine compact conversion for {targetType.FullName}");
        }
    }

    public class CompactOrderItemRecord : CompactRecord
    {
        public Guid? ProductId { get; set; }

        public string ProductName { get; set; }
    }
}
