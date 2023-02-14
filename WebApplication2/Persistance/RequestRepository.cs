using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Domain;
using WebApplication2.Services;


namespace WebApplication2.Persistance
{  
    public class RequestRepository : IRepository<RegistrationRequest>
    {
        private readonly ApplicationDbContext _context;
        private User applicant;

        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<RegistrationRequest?> AddAsync(RegistrationRequest request)
        {
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
        }
        public async Task<List<RegistrationRequest>?> GetAllAsync()
        {
            var result = await _context.Request.ToListAsync();
            return result;
        }
        public async Task<RegistrationRequest> GetByIdAsync(int id)
        {
            var result = await _context.Request.Where(r => r.Id == id).FirstOrDefaultAsync();
            return result;
        }
        public async Task<RegistrationRequest> UpdateAsync(RegistrationRequest request)
        {
            try
            {
                _context.Request.Update(request);
                await _context.SaveChangesAsync();
                return request;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<RegistrationRequest> DeleteAsync(int id)
        {
            try
            {
                var result = await _context.Request.Where(e =>e.Id == id).FirstOrDefaultAsync();
                if (id != null)
                {
                    _context.Request.Remove(result);
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