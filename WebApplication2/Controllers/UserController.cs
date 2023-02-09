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
        private readonly IRepository<User> _userRepository;

        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userRepository.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public async Task<List<User>> GetAll()
        {
            return await _userRepository.GetAllAsync();
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
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
                return View(user);
            }
            if (user.IsActive == true) 
            {
                return View(user);
            }
            await _userRepository.AddAsync(user);

            return RedirectToAction(nameof(Index));
         
        }
    }
}

        