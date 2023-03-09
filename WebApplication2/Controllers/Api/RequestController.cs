using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebApplication2.Domain;
using WebApplication2.Resources;
using WebApplication2.Services;

namespace WebApplication2.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRepository<RegistrationRequest> _requestRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IStringLocalizer<Localizer> _sharedResourceLocalizer;
        public RequestController(IRepository<RegistrationRequest> requestRepository, IRepository<User> userRepository,IStringLocalizer<Localizer> sharedResourceLocalizer)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
            _sharedResourceLocalizer = sharedResourceLocalizer;
        }
        //watch user's request with httpcontext
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
        public async Task<IActionResult> Update([FromForm] RequestCondition condition, int id)
        {
            //if (condition == null)
            //    return BadRequest();
            RegistrationRequest request = await _requestRepository.GetByIdAsync(id);

            if (request == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (condition == RequestCondition.Accepted)
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
            List<RegistrationRequest> RejectedRequests = new List<RegistrationRequest>();
            if (request.Condition == RequestCondition.Rejected)
            {
                RejectedRequests.Add(request);
                return Ok(_sharedResourceLocalizer["RejectedRequest"].Value);
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