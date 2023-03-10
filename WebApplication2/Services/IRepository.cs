
namespace AdeiesApplication.Services
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T model);
        Task<List<T>> GetAllAsync();
        Task<T> UpdateAsync(T model);
        Task<T> GetByIdAsync(int id);
        Task<T> DeleteAsync(int id);
    }
}
