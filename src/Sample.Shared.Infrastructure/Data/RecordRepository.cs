namespace Sample.Shared.Infrastructure.Data
{
    using System.Collections.Generic;
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

        public async Task<T> AddAsync<T>(T entity) where T : RecordBase
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync<T>(T entity) where T : RecordBase
        {
            _dbContext.Set<T>().Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        public T GetById<T>(int id) where T : RecordBase
        {
            return _dbContext
                .Set<T>()
                .SingleOrDefault(e => e.Id == id);
        }

        public Task<T> GetByIdAsync<T>(int id) where T : RecordBase
        {
            return _dbContext
                .Set<T>()
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public Task<List<T>> ListAsync<T>() where T : RecordBase
        {
            return _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : RecordBase
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }
    }
}
