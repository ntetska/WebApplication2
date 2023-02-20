using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Domain;
using WebApplication2.Services;

namespace WebApplication2.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRepository<RegistrationRequest> _requestRepository;
        private readonly IRepository<User> _userRepository;

        public RequestController(IRepository<RegistrationRequest> requestService, IRepository<User> userRepository)
        {
            _requestRepository = requestService;
            _userRepository = userRepository;
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(RegistrationRequest request)
        //{
        //    request = await _requestRepository.AddAsync(request);

        //    if (request == null)
        //    {
        //        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        //    }

        //    return Ok(request);
        //}

        [Authorize(Roles = "Admin")]
        public string TestingAuth()
        {
            return "Success!";
        }
        [HttpPut("/Update")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(RegistrationRequest request)
        {
            request = await _requestRepository.UpdateAsync(request);

            if (request == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (request.Condition == RequestCondition.Rejected)
            {
                return Ok("the request is rejected!");
            }
            User userToBeActive = await _userRepository.GetByIdAsync(request.ApplicantId);
            userToBeActive.IsActive = true;
            userToBeActive = await _userRepository.UpdateAsync(userToBeActive);
            if (userToBeActive == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(request);
        }
        [HttpDelete,ActionName("/Delete")]
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