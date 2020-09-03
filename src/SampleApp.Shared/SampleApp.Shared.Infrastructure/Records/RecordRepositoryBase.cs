namespace SampleApp.Shared.Infrastructure.Records
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NServiceBus;
    using SampleApp.Shared.Abstractions.Records;

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

            var messages = record.AddedMessages();
            EmitMessagesAsync(messages).GetAwaiter().GetResult();

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

            var messages = record.AddedMessages();
            await EmitMessagesAsync(messages);

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

            var ievent = record.DeletedMessages();
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

            var messages = record.DeletedMessages();
            EmitMessagesAsync(messages).GetAwaiter().GetResult();

            return record;
        }

        public virtual async Task UpdateAsync<T>(T record) where T : RecordBase
        {
            DbContext.Entry(record).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            var messages = record.UpdatedMessages();
            await EmitMessagesAsync(messages);
        }

        private async Task EmitMessagesAsync(IMessage[] messages)
        {
            if (messages == null) return;

            foreach (var message in messages)
            {
                if (message is IEvent) await _messageSession.Publish(message);
                if (message is ICommand) await _messageSession.Send(message);
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
