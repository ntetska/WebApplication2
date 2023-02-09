using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using WebApplication2.Domain;
using WebApplication2.Persistance;
using WebApplication2.Services;

namespace WebApplication2.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<List<User>> GetAll()
        {
            return await _userRepository.GetAllAsync();
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user)
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

            user = await _userRepository.AddAsync(user);

            if (user == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> Update(User user)
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
        [HttpPost]
        public async Task<IActionResult> Delete(int id) 
        {
            User user = await _userRepository.DeleteAsync(id);
            return Ok(user);

        }

        //[HttpPost]
        //public async Task<IActionResult> Login(User user)
        //{
        //    if (user.Username == string.Empty)
        //    {
        //        return BadRequest("Username is needed");
        //    }
        //    if (user.Password == string.Empty)
        //    {
        //        return BadRequest("Password is needed");
        //    }



        //    return Ok(user);
        //}
    }
}
