﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AdeiesApplication.Domain;
using AdeiesApplication.Resources;
using AdeiesApplication.Persistance;

namespace AdeiesApplication.Controllers.Api
{
    [Route("backend_api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly RequestRepository _requestRepository;
        private readonly PasswordHasher<UserCreate> _passwordHasher;
        private readonly IStringLocalizer<Localizer> _sharedResourceLocalizer;
        public UserController(UserRepository userRepository, RequestRepository requestRepository, PasswordHasher<UserCreate> passwordHasher, IStringLocalizer<Localizer> sharedResourceLocalizer)
        {
            _userRepository = userRepository;
            _requestRepository = requestRepository;
            _passwordHasher = passwordHasher;
            _sharedResourceLocalizer= sharedResourceLocalizer;
        }
        /// <summary>
        /// User gets his data (username,first and last name,phone number and email) from database.
        /// </summary>
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var userCookieID = HttpContext.User.FindFirstValue("Id");
            var user = await _userRepository.GetByIdAsync(Int32.Parse(userCookieID));
            var userDto = user.ToDTO();
            return Ok(userDto);
        }
        /// <summary>
        /// Admin gets a user's data from the database, by user_id.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return BadRequest(_sharedResourceLocalizer["Error"].Value);
            }
            var userDto = user.ToDTO();
            return Ok(userDto);
        }
        /// <summary>
        /// Admin gets a list of all users from the database.
        /// </summary>
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
        /// <summary>
        /// Manager gets a list of users he manages.
        /// </summary>
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
        /// <summary>
        /// User registration with required fields username,password,firstname,lastname,phone number and email.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(UserCreate userCreate)
        {

            RegistrationRequest request = new RegistrationRequest();
            //check the required fields
            if (userCreate.Username == string.Empty)
            {
                return BadRequest(_sharedResourceLocalizer["EmptyUsername"].Value);
            }
            if (userCreate.Password == string.Empty)
            {
                return BadRequest(_sharedResourceLocalizer["EmptyPassword"].Value);
            }
            userCreate.Password = _passwordHasher.HashPassword(userCreate, userCreate.Password);

            if (userCreate.Number.IsNullOrEmpty())
            {
                return BadRequest(_sharedResourceLocalizer["EmptyPhoneNumber"].Value);
            }
            if (userCreate.Number.Length != 10)
            {
                return BadRequest(_sharedResourceLocalizer["PhoneLength"].Value);
            }
            if (userCreate.Email == string.Empty)
            {
                return BadRequest(_sharedResourceLocalizer["EmptyMail"].Value);
            }
            User user = new User();
            userCreate.ToModelCreate(user);
            user = await _userRepository.AddAsync(user);

            if (user == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            request.Applicant = user;
            request = await _requestRepository.AddAsync(request);
            var userDto = user.ToDTO();
            return Ok(userDto);
        }
        /// <summary>
        /// Admin update the role of a user and give him a manager by manager_Id.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("AUpdate/{id}")]
        public async Task<IActionResult> Update( int id , [FromForm] UserRole? role = null , [FromForm] int? managerId = null)
        {
            if (managerId == null && role == null)
            {
                return BadRequest(_sharedResourceLocalizer["Field"].Value);
            }
            //user gets a manager
            User user = await _userRepository.GetByIdAsync(id);
            if (role != null)
            {
                user.Role = role.Value;
            }
            if (managerId != null)
            {
                User manager = await _userRepository.GetByIdAsync(managerId.Value);
                user.Manager = manager;
            }
            await _userRepository.UpdateAsync(user);
            var userDto = user.ToDTO();
            return Ok(userDto);
        }
        /// <summary>
        /// User update his data (username,password,firstname,lastname,phone number and email) by his user_id.
        /// </summary>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(UserCreate userCreate, int id)
        {
            var userCookieID = HttpContext.User.FindFirstValue("Id");
            if (userCookieID != id.ToString())
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Unathorized user");
            }
            User user = await _userRepository.GetByIdAsync(id);

            //check the required fields
            if (userCreate.Username == string.Empty)
            {
                return BadRequest(_sharedResourceLocalizer["EmptyUsername"].Value);
            }
            if (userCreate.Password == string.Empty)
            {
                return BadRequest(_sharedResourceLocalizer["EmptyPassword"].Value);
            }
            userCreate.Password = _passwordHasher.HashPassword(userCreate, userCreate.Password);

            if (userCreate.Number.IsNullOrEmpty())
            {
                return BadRequest(_sharedResourceLocalizer["EmptyPhoneNumber"].Value);
            }
            if (userCreate.Number.Length != 10)
            {
                return BadRequest(_sharedResourceLocalizer["PhoneLength"].Value);
            }
            if (userCreate.Email == string.Empty)
            {
                return BadRequest(_sharedResourceLocalizer["EmptyMail"].Value);
            }
            userCreate.ToModelCreate(user);
            user = await _userRepository.UpdateAsync(user);

            if (user == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            var userDto = user.ToDTO();
            return Ok(userDto);
        }
        /// <summary>
        /// Admin delete user by user_id.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
		{
			User user = await _userRepository.DeleteAsync(id);
			if (user == null)
			{
                return BadRequest(_sharedResourceLocalizer["Error"].Value);
            }
            var userDto = user.ToDTO();
			return Ok(userDto);
		}
	}
}
