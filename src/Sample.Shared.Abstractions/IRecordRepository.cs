namespace Sample.Shared.Abstractions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRecordRepository
    {
        Task<T> AddAsync<T>(T record) where T : RecordBase;

        Task DeleteAsync<T>(T record) where T : RecordBase;

        IQueryable<T> AsQueryable<T>() where T : RecordBase;

        Task<T> RetrieveIdAsync<T>(Guid id) where T : RecordBase;

        Task UpdateAsync<T>(T record) where T : RecordBase;
    }
}
