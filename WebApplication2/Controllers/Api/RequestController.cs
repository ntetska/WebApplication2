using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.Domain;
using WebApplication2.Persistance;
using WebApplication2.Services;

namespace WebApplication2.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRepository<RegistrationRequest> _requestRepository;

        public RequestController(IRepository<RegistrationRequest> requestService)
        {
            _requestRepository = (IRepository<RegistrationRequest>?)requestService;

        }
        [HttpGet]
        public async Task<IActionResult> GetSingle(int id)
        {
            return (IActionResult)await _requestRepository.GetByIdAsync(id);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return (IActionResult)await _requestRepository.GetAllAsync();
        }
        [HttpPost]
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
        public async Task<IActionResult> Update(RegistrationRequest request)
        {
            request = await _requestRepository.UpdateAsync(request);

            if (request == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(request);
        }
        public async Task<IActionResult> Delete(int id)
        {
            RegistrationRequest request = await _requestRepository.DeleteAsync(id);
            return Ok(request);
        }

    }
}
