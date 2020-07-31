namespace Sample.Shared.Infrastructure.Data
{
    public class CosmosRecordRepository : RecordRepositoryBase
    {
        public CosmosRecordRepository(CosmosDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
