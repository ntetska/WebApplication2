using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.Domain;
using WebApplication2.Persistance;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRepository<RegistrationRequest> _requestRepository;

        public RequestController(IRepository<RegistrationRequest> requestService)
        {
            _requestRepository = requestService;

        }
        public async Task<IActionResult> GetSingle(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            return Ok(request);

        }
        public async Task<IActionResult> GetAllAsync()
        {
            var requests = await _requestRepository.GetAllAsync();
            return Ok(requests);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegistrationRequest request)
        {
            request = await _requestRepository.AddAsync(request);

            if (request == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(RegistrationRequest request)
        {
            request = await _requestRepository.UpdateAsync(request);

            if (request == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(request);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            RegistrationRequest request = await _requestRepository.DeleteAsync(id);
            if (request == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(request);
        }

    }
}