namespace SampleApp.Shared.Abstractions.Records
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRecordRepository
    {
        T Add<T>(T record) where T : RecordBase;

        Task<T> AddAsync<T>(T record) where T : RecordBase;

        IQueryable<T> AsQueryable<T>() where T : RecordBase;

        Task DeleteAsync<T>(T record) where T : RecordBase;

        T Retrieve<T>(Guid id) where T : RecordBase;

        Task<T> RetrieveAsync<T>(Guid id) where T : RecordBase;

        T Update<T>(T record) where T : RecordBase;

        Task UpdateAsync<T>(T record) where T : RecordBase;

        Task<IEnumerable<TRecord>> Query<TRecord>(Expression<Func<TRecord, bool>> predicate, int skip = 0, int take = 1000)
            where TRecord : RecordBase;

        Task<IEnumerable<TRecord>> Query<TRecord>(int skip = 0, int take = 10000) where TRecord : RecordBase;
    }
}
