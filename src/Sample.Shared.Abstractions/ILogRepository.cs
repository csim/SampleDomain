namespace Sample.Shared.Abstractions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILogRepository
    {
        Task<T> AddAsync<T>(T entity) where T : RecordBase;

        Task DeleteAsync<T>(T entity) where T : RecordBase;

        Task<List<T>> RetrieveAsync<T>() where T : RecordBase;

        Task<T> RetrieveByIdAsync<T>(int id) where T : RecordBase;

        Task UpdateAsync<T>(T entity) where T : RecordBase;
    }
}
