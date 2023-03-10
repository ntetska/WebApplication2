using Microsoft.EntityFrameworkCore;
using AdeiesApplication.Domain;

namespace AdeiesApplication.Persistance
{
    public class UserRepository 
    {
        private readonly ApplicationDbContext _context;

        public object FirstName { get; private set; }

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User> AddAsync(User user)
        {
            try
            {
                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<User>> GetAllAsync()
        { 
            var result = await _context.User
                .Include(u => u.Vacation)
                .ToListAsync();
            return result;
        }
        //TO DO
        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _context.User.Where(u => u.Id == id).Include(u => u.Manager).FirstOrDefaultAsync();
            return user;
        }
        public async Task<User> GetByUsernameAsync(string username)
        {
            var result = await _context.User.Where(u => u.Username == username).FirstOrDefaultAsync();
            return result;
        }
        public async Task<User?> UpdateAsync(User user)
        {
            try
            {
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<User> DeleteAsync(int id)
        {
            try
            {
                var result = await _context.User.Where(u => u.Id == id).FirstOrDefaultAsync();
                if (result != null)
                {
                    _context.User.Remove(result);
                    await _context.SaveChangesAsync();
                }
                return result;

            }
            catch (Exception ex)
            {

                return null;
            }

        }
    }
}