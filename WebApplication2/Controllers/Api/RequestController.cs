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

        public RequestController(IRepository<RegistrationRequest> requestRepository, IRepository<User> userRepository)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            return Ok(request);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var requests = await _requestRepository.GetAllAsync();
            return Ok(requests);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromForm] bool? toukan, int id)
        {
            if (toukan == null)
                return BadRequest();
            RegistrationRequest request = await _requestRepository.GetByIdAsync(id);

            if (request == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (request.Condition != RequestCondition.Pending)
            {
                return BadRequest("The request is not pending");
            }
            if (toukan == true)
            {
                request.Condition = RequestCondition.Accepted;
                User userToBeActive = await _userRepository.GetByIdAsync(request.ApplicantId);
                userToBeActive.IsActive = true;
                userToBeActive = await _userRepository.UpdateAsync(userToBeActive);
                if (userToBeActive == null)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                request.Condition = RequestCondition.Rejected;  
            }

            request = await _requestRepository.UpdateAsync(request);

            if (request.Condition == RequestCondition.Rejected)
            {
                return Ok("the request is rejected!");
            }
           
            return Ok(request);
        }
        [HttpDelete("Delete/{id}")]
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