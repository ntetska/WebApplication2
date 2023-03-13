using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using AdeiesApplication.Domain;



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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                await _context.Vacation.AddAsync(vacation);
                await _context.SaveChangesAsync();
                return vacation;
            }
            catch (Exception ex)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
        }
        public async Task<List<Vacation>?> GetAllAsync()
        {
            var result = await _context.Vacation.Include(x => x.Petitioner).ThenInclude(x => x.Manager).ToListAsync();
            //var result = await _context.Vacation.ToListAsync();
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
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
#pragma warning restore CS0168 // The variable 'ex' is declared but never used

        }
        public async Task<Vacation?> DeleteAsync(int id)
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                var vacation = await _context.Vacation.Where(v => v.Id == id).FirstOrDefaultAsync();
#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
                if (id != null)
                {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'entity' in 'EntityEntry<Vacation> DbSet<Vacation>.Remove(Vacation entity)'.
                    _context.Vacation.Remove(vacation);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'entity' in 'EntityEntry<Vacation> DbSet<Vacation>.Remove(Vacation entity)'.
                    await _context.SaveChangesAsync();
                }
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
                return vacation;
            }
            catch (Exception ex)
            {

                return null;
            }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used

        }
    }
}