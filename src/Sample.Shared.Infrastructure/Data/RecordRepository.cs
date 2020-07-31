namespace Sample.Shared.Infrastructure.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Sample.Shared.Abstractions;

    public class RecordRepository : IRecordRepository
    {
        public RecordRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly AppDbContext _dbContext;

        public async Task<T> AddAsync<T>(T record) where T : RecordBase
        {
            record.Id ??= Guid.NewGuid();

            await _dbContext.Set<T>().AddAsync(record);
            await _dbContext.SaveChangesAsync();

            return record;
        }

        public IQueryable<T> AsQueryable<T>() where T : RecordBase
        {
            return _dbContext
                .Set<T>()
                .AsQueryable<T>();
        }

        public async Task DeleteAsync<T>(T record) where T : RecordBase
        {
            _dbContext.Set<T>().Remove(record);

            await _dbContext.SaveChangesAsync();
        }

        public T RetrieveId<T>(Guid id) where T : RecordBase
        {
            return _dbContext
                .Set<T>()
                .SingleOrDefault(e => e.Id == id);
        }

        public Task<T> RetrieveIdAsync<T>(Guid id) where T : RecordBase
        {
            return _dbContext
                .Set<T>()
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateAsync<T>(T record) where T : RecordBase
        {
            _dbContext.Entry(record).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }
    }
}
