namespace SampleApp.Shared.Infrastructure.Data
{
    using NServiceBus;

    public class SqlServerRecordRepository : RecordRepositoryBase
    {
        public SqlServerRecordRepository(SqlServerDbContext dbContext, IMessageSession messageSession) : base(messageSession)
        {
            DbContext = dbContext;
        }
    }
}
