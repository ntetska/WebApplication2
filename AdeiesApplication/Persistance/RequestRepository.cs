using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdeiesApplication.Domain;

namespace AdeiesApplication.Persistance
{  
    public class RequestRepository
    {
        private readonly ApplicationDbContext _context;
#pragma warning disable CS0169 // The field 'RequestRepository.applicant' is never used
        private User applicant;
#pragma warning restore CS0169 // The field 'RequestRepository.applicant' is never used

#pragma warning disable CS8618 // Non-nullable field 'applicant' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
        public RequestRepository(ApplicationDbContext context)
#pragma warning restore CS8618 // Non-nullable field 'applicant' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
        {
            _context = context;
        }
        public async Task<RegistrationRequest?> AddAsync(RegistrationRequest request)
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                await _context.Request.AddAsync(request);
                await _context.SaveChangesAsync();
                return request;
            }
            catch (Exception ex)
            {
                return null;
            }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
        }
        public async Task<List<RegistrationRequest>?> GetAllAsync()
        {
            var result = await _context.Request.ToListAsync();
            return result;
        }
        public async Task<RegistrationRequest> GetByIdAsync(int id)
        {
            var result = await _context.Request.Where(r => r.Id == id).FirstOrDefaultAsync();
#pragma warning disable CS8603 // Possible null reference return.
            return result;
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<RegistrationRequest> UpdateAsync(RegistrationRequest request)
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                _context.Request.Update(request);
                await _context.SaveChangesAsync();
                return request;
            }
            catch (Exception ex)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used

        }
        public async Task<RegistrationRequest> DeleteAsync(int id)
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                var result = await _context.Request.Where(e =>e.Id == id).FirstOrDefaultAsync();
#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
                if (id != null)
                {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'entity' in 'EntityEntry<RegistrationRequest> DbSet<RegistrationRequest>.Remove(RegistrationRequest entity)'.
                    _context.Request.Remove(result);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'entity' in 'EntityEntry<RegistrationRequest> DbSet<RegistrationRequest>.Remove(RegistrationRequest entity)'.
                    await _context.SaveChangesAsync();
                }
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
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