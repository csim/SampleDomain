namespace SampleApp.Shared.Infrastructure.Data
{
    using NServiceBus;

    public class CosmosRecordRepository : RecordRepositoryBase
    {
        public CosmosRecordRepository(CosmosDbContext dbContext, IMessageSession messageSession) : base(messageSession)
        {
            DbContext = dbContext;
        }
    }
}
