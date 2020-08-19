namespace SampleApp.Shared.Infrastructure.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NServiceBus;
    using SampleApp.Shared.Abstractions;

    public abstract class RecordRepositoryBase : IRecordRepository
    {
        protected RecordRepositoryBase(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        private readonly IMessageSession _messageSession;

        protected DbContext DbContext { get; set; }

        public virtual T Add<T>(T record) where T : RecordBase
        {
            record.Id ??= Guid.NewGuid();
            record.CreatedOn ??= DateTime.UtcNow;
            record.ModifiedOn ??= DateTime.UtcNow;

            DbContext
                .Set<T>()
                .Add(record);

            DbContext.SaveChanges();

            var ievent = record.AddedEvent();
            if (ievent != null)
            {
                _messageSession.Publish(ievent).GetAwaiter().GetResult();
            }

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

            var ievent = record.AddedEvent();
            if (ievent != null)
            {
               await _messageSession.Publish(ievent);
            }

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

            var ievent = record.DeletedEvent();
            if (ievent != null)
            {
                await _messageSession.Publish(ievent);
            }
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

            var ievent = record.DeletedEvent();
            if (ievent != null)
            {
                _messageSession.Publish(ievent).GetAwaiter().GetResult();
            }

            return record;
        }

        public virtual async Task UpdateAsync<T>(T record) where T : RecordBase
        {
            DbContext.Entry(record).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            var ievent = record.UpdatedEvent();
            if (ievent != null)
            {
                await _messageSession.Publish(ievent);
            }
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
