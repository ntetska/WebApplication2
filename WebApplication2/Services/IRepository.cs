using WebApplication2.Domain;

namespace WebApplication2.Services
{
    public interface IRepository<T>
    {
        Task<bool> AddAsync(T model);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(string username);
    }
}
