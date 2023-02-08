using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using WebApplication2.Domain;
using WebApplication2.Models;
using WebApplication2.Services;


namespace WebApplication2.Controllers
{
    public class UserController
        : Controller
    {
        private readonly  IRepository<User> _userRepository;

        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userRepository.GetAllAsync());
        }

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,FirstName,LastName,Number,Email,Username,Password,Role,Manager_Id")] Domain.User user, object firstName)
        {

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if(user.Password == null)
            {
                return View(user);
            }

            await _userRepository.AddAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,Username,Password")] Domain.User user)
        {

            if (ModelState.IsValid)
            {
                await _userRepository.AddAsync(user);

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}

        