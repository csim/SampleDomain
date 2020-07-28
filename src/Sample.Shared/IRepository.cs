namespace Sample.Shared
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository
    {
        Task<T> AddAsync<T>(T entity) where T : EntityBase;

        Task DeleteAsync<T>(T entity) where T : EntityBase;

        Task<T> GetByIdAsync<T>(int id) where T : EntityBase;

        Task<List<T>> ListAsync<T>() where T : EntityBase;

        Task UpdateAsync<T>(T entity) where T : EntityBase;
    }
}
