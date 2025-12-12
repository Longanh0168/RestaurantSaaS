using System.Linq.Expressions;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        // Hàm tìm kiếm linh hoạt
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}