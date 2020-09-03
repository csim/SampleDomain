namespace SampleApp.Shared.Infrastructure.Records.Orders
{
    using NServiceBus;
    using SampleApp.Orders.Client.Records;

    public class OrdersRecordRepository : RecordRepositoryBase, IOrdersRecordRepository
    {
        public OrdersRecordRepository(OrdersDbContext dbContext, IMessageSession messageSession) : base(messageSession)
        {
            DbContext = dbContext;
        }
    }
}
