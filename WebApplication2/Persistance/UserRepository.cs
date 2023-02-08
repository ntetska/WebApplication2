using Microsoft.EntityFrameworkCore;
using WebApplication2.Domain;
using WebApplication2.Services;


namespace WebApplication2.Persistance
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationDbContext _context;

        public object FirstName { get; private set; }

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(User user)
        {
            try
            {
                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<User>> GetAllAsync()
        {
            var result = await _context.User.ToListAsync();
            return result;
        }
        public async Task<User> GetByIdAsync(string username)
        {
            var result = await _context.User.Where(e => e.Username == username).FirstOrDefaultAsync();
            return result;
        }
        public async Task<bool> LoginAsync(User user)
        {
            try
            {
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}