using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.Domain;
using WebApplication2.Services;

namespace WebApplication2.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<RegistrationRequest> _requestRepository;
        public UserController(IRepository<User> userRepository, IRepository<RegistrationRequest> requestRepository)
        {
            _userRepository = userRepository;
            _requestRepository = requestRepository;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return Ok(user);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,FirstName,LastName,Number,Email")] User user)
        {
            RegistrationRequest request = new RegistrationRequest();

            if (user.Username == string.Empty)
            {
                return BadRequest("Username is needed");
            }
            if (user.Password == string.Empty)
            {
                return BadRequest("Password is needed");
            }
            if (user.Number.IsNullOrEmpty())
            {
                return BadRequest("Number is needed");
            }
            if (user.Number.Length != 10)
            {
                return BadRequest("Number must be a length of 10");
            }
            if (user.Email == string.Empty)
            {
                return BadRequest("Email is needed");
            }

            user = await _userRepository.AddAsync(user);

            if (user == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            request.Applicant = user;
            request = await _requestRepository.AddAsync(request);
            if (ModelState.IsValid)
            {
                await _userRepository.AddAsync(user);

                return RedirectToAction(nameof(Index));
            }
            return Ok(user);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([Bind("Id,Username,Password,FirstName,LastName,Number,Email")] User user)
        {
            if (user.Username == string.Empty)
            {
                return BadRequest("Username is needed");
            }
            if (user.Password == string.Empty)
            {
                return BadRequest("Password is needed");
            }
            if (user.Number.IsNullOrEmpty())
            {
                return BadRequest("Number is needed");
            }
            if (user.Number.Length != 10)
            {
                return BadRequest("Number must be a length of 10");
            }
            if (user.Email == string.Empty)
            {
                return BadRequest("Email is needed");
            }

            user = await _userRepository.UpdateAsync(user);

            if (user == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(user);
        }
		[HttpDelete("Delete/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			User user = await _userRepository.DeleteAsync(id);
			if (user == null)
			{
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
			return Ok(user);
		}
	}
}
