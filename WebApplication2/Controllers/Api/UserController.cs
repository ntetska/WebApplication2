using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WebApplication2.Domain;
using WebApplication2.Services;

namespace WebApplication2.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<RegistrationRequest> _requestRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IStringLocalizer<UserController> _sharedResourceLocalizer;
        public UserController(IRepository<User> userRepository, IRepository<RegistrationRequest> requestRepository, PasswordHasher<User> passwordHasher, IStringLocalizer<UserController> sharedResourceLocalizer)
        {
            _userRepository = userRepository;
            _requestRepository = requestRepository;
            _passwordHasher = passwordHasher;
            _sharedResourceLocalizer= sharedResourceLocalizer;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var userCookieID = HttpContext.User.FindFirstValue("Id");
            var user = await _userRepository.GetByIdAsync(Int32.Parse(userCookieID));
            var userDto = user.ToDTO();
            return Ok(userDto);
        }

        [Authorize(Roles = "Admin")]
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
            List<UserDTO> userDTOs = new List<UserDTO>();
            foreach (var user in users)
            {
                var userDto = user.ToDTO();
                userDTOs.Add(userDto);
            }
            return Ok(userDTOs);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("GetManagedUsers")]
        public async Task<IActionResult> GetAllAsync()
        {
            List<UserDTO> ManagedUsers = new List<UserDTO>();

            var userCookieID = HttpContext.User.FindFirstValue("Id");

            var users = await _userRepository.GetAllAsync();

            foreach (var user in users) 
            {
                if (user.Manager != null && (user.Manager.Id == (int.Parse(userCookieID))))
                {
                    var userDto = user.ToDTO();
                    ManagedUsers.Add(userDto);
                }
            }
            return Ok(ManagedUsers);
        }
        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] User user)
        {
            

            RegistrationRequest request = new RegistrationRequest();
            //check the required fields
            if (user.Username == string.Empty)
            {
                return BadRequest(_sharedResourceLocalizer["EmptyUsername"].Value);
            }
            if (user.Password == string.Empty)
            {
                return BadRequest(_sharedResourceLocalizer["EmptyPassword"].Value);
            }

            user.Password = _passwordHasher.HashPassword(user,user.Password);

            if (user.Number.IsNullOrEmpty())
            {
                return BadRequest(_sharedResourceLocalizer["EmptyPhoneNumber"].Value);
            }
            if (user.Number.Length != 10)
            {
                return BadRequest(_sharedResourceLocalizer["PhoneLength"].Value);
            }
            if (user.Email == string.Empty)
            {
                return BadRequest(_sharedResourceLocalizer["EmptyMail"].Value);
            }

            user = await _userRepository.AddAsync(user);

            if (user == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            request.Applicant = user;
            request = await _requestRepository.AddAsync(request);
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("AUpdate/{id}")]
        public async Task<IActionResult> Update([FromForm] UserRole Role, [FromForm] int managerId , int id)
        {
            //user gets a manager
            User user = await _userRepository.GetByIdAsync(id);
            user.Role = Role;
            User manager = await _userRepository.GetByIdAsync(managerId);
            user.Manager = manager;
            await _userRepository.UpdateAsync(user);
            return Ok();
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(User user, int id)
        {
            var userCookieID = HttpContext.User.FindFirstValue("Id");
            if (userCookieID != id.ToString())
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Unathorized user");
            }
            //var user = await _userRepository.GetByIdAsync(Int32.Parse(id));
            user = await _userRepository.GetByIdAsync(id);

            //check the required fields
            if (user.Username == string.Empty)
            {
                return BadRequest(_sharedResourceLocalizer["EmptyUsername"].Value);
            }
            if (user.Password == string.Empty)
            {
                return BadRequest(_sharedResourceLocalizer["EmptyPassword"].Value);
            }
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            if (user.Number.IsNullOrEmpty())
            {
                return BadRequest(_sharedResourceLocalizer["EmptyPhoneNumber"].Value);
            }
            if (user.Number.Length != 10)
            {
                return BadRequest(_sharedResourceLocalizer["PhoneLength"].Value);
            }
            if (user.Email == string.Empty)
            {
                return BadRequest(_sharedResourceLocalizer["EmptyMail"].Value);
            }
            user = await _userRepository.UpdateAsync(user);

            if (user == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            var userDto = user.ToDTO();
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
		{
			User user = await _userRepository.DeleteAsync(id);
			if (user == null)
			{
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
            var userDto = user.ToDTO();
			return Ok(userDto);
		}
	}
}
