using Microsoft.AspNetCore.Mvc;
using Nest;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using WebApplication2.Domain;
using WebApplication2.Models;
using WebApplication2.Services;


namespace WebApplication2.Controllers
{
    [ApiController]
    public class UserController
        : Controller
    {
        private readonly Services.IRepository<User> _userService;

        public UserController(Services.IRepository<User> userService)
        {
            _userService = userService;
        }

        [HttpGet("/User")]
        public async Task<IActionResult> Index()
        {

            return View(await _userService.GetAllAsync());
        }

        [HttpGet("/User/Register")]
        public string Register()
        {
            return "You have registered!";
            //return View();
        }

        [HttpPost("/User/Register")]
        [ValidateAntiForgeryToken]
        public string Register(User user)
        {

            //if (ModelState.IsValid)
            //{
            //    await _userService.AddAsync(user);

            //    return RedirectToAction(nameof(Index));
            //}
            return "Success!";
        }
        [HttpGet("/User/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/User/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,Username,Password")] Domain.User user)
        {

            return BadRequest("Wrong password");
            if (ModelState.IsValid)
            {
                await _userService.AddAsync(user);

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}

        