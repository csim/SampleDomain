namespace Sample.Shared.Abstractions
{
    using System;
    using System.Linq;
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
    }
}
