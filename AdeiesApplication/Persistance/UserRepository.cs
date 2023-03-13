using Microsoft.EntityFrameworkCore;
using AdeiesApplication.Domain;

namespace AdeiesApplication.Persistance
{
    public class UserRepository 
    {
        private readonly ApplicationDbContext _context;

        public object FirstName { get; private set; }

#pragma warning disable CS8618 // Non-nullable property 'FirstName' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public UserRepository(ApplicationDbContext context)
#pragma warning restore CS8618 // Non-nullable property 'FirstName' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        {
            _context = context;
        }
        public async Task<User> AddAsync(User user)
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS8603 // Possible null reference return.
            return user;
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<User> GetByUsernameAsync(string username)
        {
            var result = await _context.User.Where(u => u.Username == username).FirstOrDefaultAsync();
#pragma warning disable CS8603 // Possible null reference return.
            return result;
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<User?> UpdateAsync(User user)
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
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
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
        }
        public async Task<User> DeleteAsync(int id)
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                var result = await _context.User.Where(u => u.Id == id).FirstOrDefaultAsync();
                if (result != null)
                {
                    _context.User.Remove(result);
                    await _context.SaveChangesAsync();
                }
#pragma warning disable CS8603 // Possible null reference return.
                return result;
#pragma warning restore CS8603 // Possible null reference return.

            }
            catch (Exception ex)
            {

#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used

        }
    }
}