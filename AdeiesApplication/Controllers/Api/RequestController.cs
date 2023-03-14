using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using AdeiesApplication.Domain;
using AdeiesApplication.Resources;
using AdeiesApplication.Persistance;

namespace AdeiesApplication.Controllers.Api
{
    [Route("backend_api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly RequestRepository _requestRepository;
        private readonly UserRepository _userRepository;
        private readonly IStringLocalizer<Localizer> _sharedResourceLocalizer;
        public RequestController(RequestRepository requestRepository, UserRepository userRepository,IStringLocalizer<Localizer> sharedResourceLocalizer)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
            _sharedResourceLocalizer = sharedResourceLocalizer;
        }
        /// <summary>
        /// Admin gets a registration request of a user by request_id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSingle(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            return Ok(request);
        }
        /// <summary>
        /// Admin gets a list of all requests from the database.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var requests = await _requestRepository.GetAllAsync();
            return Ok(requests);
        }
        /// <summary>
        /// Admin update request condition by request_id and user gets his hire date. The condition has default value of pending and can get the value of accepted and rejected.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromForm] RequestCondition condition,[FromForm] DateOnly hiredate, int id)
        {
            //if (condition == null)
            //    return BadRequest();
            RegistrationRequest request = await _requestRepository.GetByIdAsync(id);

            if (request == null)
            {
                return BadRequest(_sharedResourceLocalizer["Error"].Value);
            }

            if (condition == RequestCondition.Accepted)
            {
                request.Condition = RequestCondition.Accepted;
                User userToBeActive = await _userRepository.GetByIdAsync(request.ApplicantId);
                userToBeActive.IsActive = true;
                userToBeActive.HireDate = hiredate.ToDateTime(new TimeOnly(0, 0));
                userToBeActive = await _userRepository.UpdateAsync(userToBeActive);
                if (userToBeActive == null)
                {
                    return BadRequest(_sharedResourceLocalizer["Error"].Value);
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
        /// <summary>
        /// Admin delete the request by request_id.
        /// </summary>
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            RegistrationRequest request = await _requestRepository.DeleteAsync(id);
            if (request == null)
            {
                return BadRequest(_sharedResourceLocalizer["Error"].Value);
            }
            return Ok(request);
        }
    }
}