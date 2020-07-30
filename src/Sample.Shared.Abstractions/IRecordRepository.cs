namespace Sample.Shared.Abstractions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRecordRepository
    {
        Task<T> AddAsync<T>(T entity) where T : RecordBase;

        Task DeleteAsync<T>(T entity) where T : RecordBase;

        Task<T> GetByIdAsync<T>(int id) where T : RecordBase;

        Task<List<T>> ListAsync<T>() where T : RecordBase;

        Task UpdateAsync<T>(T entity) where T : RecordBase;
    }
}
