using WebApplication2.Domain;

namespace WebApplication2.Services
{
    public interface IRepository<T>
    {
        Task<bool> AddAsync(T model);
        Task<List<T>> GetAllAsync();
        Task<bool> UpdateAsync(T model);
        Task<T> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
