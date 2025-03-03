using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Models.Repositories
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveAsync();
    }
}
