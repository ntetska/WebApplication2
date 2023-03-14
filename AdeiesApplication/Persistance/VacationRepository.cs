using Microsoft.EntityFrameworkCore;
using AdeiesApplication.Domain;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdeiesApplication.Persistance
{
    public class VacationRepository 
    {
        private readonly ApplicationDbContext _context;

        public VacationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Vacation> AddAsync(Vacation vacation)
        {
            try
            {
                await _context.Vacation.AddAsync(vacation);
                await _context.SaveChangesAsync();
                return vacation;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<Vacation>?> GetAllAsync()
        {
            var result = await _context.Vacation.Include(x => x.Petitioner).ThenInclude(x => x.Manager).ToListAsync();
            return result;
        }
        public async Task<List<Vacation>?> GetMyVacationsAsync()
        {
            var MyVacations = await _context.Vacation.Include(x => x.Petitioner).ToListAsync();
            return MyVacations;
        }
        public async Task<Vacation?> GetByIdAsync(int id)
        {
            var result = await _context.Vacation.Where(v => v.Id == id).Include(x => x.Petitioner).ThenInclude(x => x.Manager).FirstOrDefaultAsync();
            return result;
        }
        public async Task<Vacation?> UpdateAsync(Vacation vacation)
        {
            try
            {
                _context.Vacation.Update(vacation);
                await _context.SaveChangesAsync();
                return vacation;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<Vacation?> DeleteAsync(int id)
        {
            try
            {
                var vacation = await _context.Vacation.Where(v => v.Id == id).FirstOrDefaultAsync();
                if (id != null)
                {
                    _context.Vacation.Remove(vacation);
                    await _context.SaveChangesAsync();
                }
                return vacation;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}