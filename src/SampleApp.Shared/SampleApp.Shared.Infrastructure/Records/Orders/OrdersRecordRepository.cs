namespace SampleApp.Shared.Infrastructure.Records.Orders
{
    using NServiceBus;
    using SampleApp.Orders.Client.Data;

    public class OrdersRecordRepository : RecordRepositoryBase, IOrdersRecordRepository
    {
        public OrdersRecordRepository(OrdersDbContext dbContext, IMessageSession messageSession) : base(messageSession)
        {
            DbContext = dbContext;
        }
    }
}
