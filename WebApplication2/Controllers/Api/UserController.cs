using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly PasswordHasher<User> _passwordHasher;
        public UserController(IRepository<User> userRepository, IRepository<RegistrationRequest> requestRepository, PasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _requestRepository = requestRepository;
            _passwordHasher = passwordHasher;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return Ok(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(User user)
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

            //user.Password = _userRepository.HashPassword(user.Password);
            user.Password = _passwordHasher.HashPassword(user, user.Password);

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
            //if (ModelState.IsValid)
            //{
            //await _userRepository.AddAsync(user);

            //    return RedirectToAction(nameof(Index));
            //}
            return Ok(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("AUpdate/{id}")]
        public async Task<IActionResult> Update([FromForm] UserRole? userRole, [FromForm] int? managerId , int id)
        {
            if (userRole == null && managerId == null)
            {
                return BadRequest("null");
            }
            User user = await _userRepository.GetByIdAsync(id);
            user.Role = userRole.Value;
            User manager = await _userRepository.GetByIdAsync(managerId.Value);
            user.Manager = manager;
            await _userRepository.UpdateAsync(user);
            return Ok();
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(UserDTO userDto, int id)
        {
            if (userDto.Username == string.Empty)
            {
                return BadRequest("Username is needed");
            }
            if (userDto.Password == string.Empty)
            {
                return BadRequest("Password is needed");
            }
            if (userDto.Number.IsNullOrEmpty())
            {
                return BadRequest("Number is needed");
            }
            if (userDto.Number.Length != 10)
            {
                return BadRequest("Number must be a length of 10");
            }
            if (userDto.Email == string.Empty)
            {
                return BadRequest("Email is needed");
            }
            User user = await _userRepository.GetByIdAsync(id);
            userDto.ToModel(user);

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
