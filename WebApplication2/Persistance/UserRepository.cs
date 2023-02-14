using Microsoft.AspNetCore.Mvc;
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
        public async Task<User> AddAsync(User user)
        {
            try
            {
                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<User>> GetAllAsync()
        {
            var result = await _context.User.ToListAsync();
            return result;
        }
        public async Task<User> GetByIdAsync(int id)
        {
            var result = await _context.User.Where(e => e.Id == id).FirstOrDefaultAsync();
            return result;
        }
        public async Task<User> UpdateAsync(User user)
        {
            try
            {
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch
            {
                return null;
            }

        }
        public async Task<User> DeleteAsync(int id)
        {
            try
            {
                var result = await _context.User.Where(k => k.Id == id).FirstOrDefaultAsync();
                if (result != null)
                {
                    _context.User.Remove(result);
                    await _context.SaveChangesAsync();
                }
                return result;

            }
            catch
            {

                return null;
            }

        }

    }
}