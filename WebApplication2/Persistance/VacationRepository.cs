using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using WebApplication2.Domain;
using WebApplication2.Services;




namespace WebApplication2.Persistance
{
    public class VacationRepository : IRepository<Vacation>
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
            var result = await _context.Vacation.ToListAsync();
            return result;
        }
        public async Task<Vacation> GetByIdAsync(int id)
        {
            var result = await _context.Vacation.Where(v => v.Id == id).Include(x => x.Petitioner).ThenInclude(x => x.managerId).FirstOrDefaultAsync();
            return result;
        }
        public async Task<Vacation> UpdateAsync(Vacation vacation)
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
        public async Task<Vacation> DeleteAsync(int id)
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