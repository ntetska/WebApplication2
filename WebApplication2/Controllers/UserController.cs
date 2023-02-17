using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System;
using System.Net;
using WebApplication2.Domain;
using WebApplication2.Persistance;
using WebApplication2.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;

namespace WebApplication2.Controllers
{
    public class UserController : Controller
    {
        //private readonly ILogger _logger;

        //public UserController(ILogger<UserController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<RegistrationRequest> _requestRepository;
        public UserController(IRepository<User> userRepository, IRepository<RegistrationRequest> requestRepository)
        {
            _userRepository = userRepository;
            _requestRepository = requestRepository;
        }

        public async Task<IActionResult> GetSingle(int id)
        {
            return View(await _userRepository.GetByIdAsync(id));
        }
        public async Task<IActionResult> Index()
        {
            return View(await _userRepository.GetAllAsync());
        }
        [HttpGet("/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            return View(user);
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            return View(user);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userRepository.DeleteAsync(id);
            //if (user == null)
            //{
            //    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            //}
            return RedirectToAction(nameof(Index));
        }


        //[HttpPost("User/Logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    _logger.LogInformation("User {Name} logged out at {Time}.",
        //        User.Identity.Name, DateTime.UtcNow);

        //    #region snippet1
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    #endregion

        //    return RedirectToPage("/Account/SignedOut");
        //}
    }
}

