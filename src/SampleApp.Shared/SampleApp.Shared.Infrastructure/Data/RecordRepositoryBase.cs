namespace SampleApp.Shared.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SampleApp.Shared.Abstractions;

    public abstract class RecordRepositoryBase : IRecordRepository
    {
        protected DbContext DbContext { get; set; }

        protected Dictionary<string, IRecordEvent> EventLookup { get; } = new Dictionary<string, IRecordEvent>();

        protected Dictionary<string, IRecordEvent> EventRegistrations { get; } = new Dictionary<string, IRecordEvent>();

        public virtual T Add<T>(T record) where T : RecordBase
        {
            record.Id ??= Guid.NewGuid();
            record.CreatedOn ??= DateTime.UtcNow;
            record.ModifiedOn ??= DateTime.UtcNow;

            DbContext
                .Set<T>()
                .Add(record);

            DbContext.SaveChanges();

            return record;
        }

        public virtual async Task<T> AddAsync<T>(T record) where T : RecordBase
        {
            record.Id ??= Guid.NewGuid();
            record.CreatedOn ??= DateTime.UtcNow;
            record.ModifiedOn ??= DateTime.UtcNow;

            await DbContext
                .Set<T>()
                .AddAsync(record);

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
            DbContext
                .Set<T>()
                .Remove(record);

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

        public virtual T Update<T>(T record) where T : RecordBase
        {
            record.Id ??= Guid.NewGuid();

            DbContext
                .Set<T>()
                .Update(record);

            DbContext.SaveChanges();

            return record;
        }

        public virtual async Task UpdateAsync<T>(T record) where T : RecordBase
        {
            DbContext.Entry(record).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();
        }
    }

    public class RecordEventRegistration
    {
        public RecordEventRegistration(RecordOperation operation, Type eventType)
        {
            Operation = operation;
            EventType = eventType;
        }

        public Type EventType { get; set; }

        public RecordOperation Operation { get; set; }
    }

    public enum RecordOperation
    {
        Add,
        Retrieve,
        Update,
        Delete,
    }
}
