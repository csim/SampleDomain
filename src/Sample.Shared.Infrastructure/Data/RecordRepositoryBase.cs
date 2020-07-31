namespace Sample.Shared.Infrastructure.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Sample.Shared.Abstractions;

    public abstract class RecordRepositoryBase : IRecordRepository
    {
        protected DbContext DbContext { get; set; }

        public virtual async Task<T> AddAsync<T>(T record) where T : RecordBase
        {
            record.Id ??= Guid.NewGuid();

            await DbContext.Set<T>().AddAsync(record);
            await DbContext.SaveChangesAsync();

            return record;
        }

        public virtual IQueryable<T> AsQueryable<T>() where T : RecordBase
        {
            return DbContext
                .Set<T>()
                .AsQueryable<T>();
        }

        public virtual async Task DeleteAsync<T>(T record) where T : RecordBase
        {
            DbContext.Set<T>().Remove(record);

            await DbContext.SaveChangesAsync();
        }

        public virtual T Retrieve<T>(Guid id) where T : RecordBase
        {
            return DbContext
                .Set<T>()
                .SingleOrDefault(e => e.Id == id);
        }

        public virtual Task<T> RetrieveAsync<T>(Guid id) where T : RecordBase
        {
            return DbContext
                .Set<T>()
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task UpdateAsync<T>(T record) where T : RecordBase
        {
            DbContext.Entry(record).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();
        }
    }
}
