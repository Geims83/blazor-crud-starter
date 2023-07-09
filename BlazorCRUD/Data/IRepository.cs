using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCRUD.Data.Models;

namespace BlazorCRUD.Data
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(long id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(long id);
    }
}