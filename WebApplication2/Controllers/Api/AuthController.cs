using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication2.Persistance;
using WebApplication2.Services;
using WebApplication2.Domain;
using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Controllers.Api;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;
    public AuthController(UserRepository userRepository, PasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
    {

        var user = await _userRepository.GetByUsernameAsync(username);
        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        if(result == PasswordVerificationResult.Failed)
        {
            return BadRequest("different password");
        }

        if (user.IsActive == false)
        {
            return BadRequest("user is not active");
        }

        var claims = new List<Claim>
        {
            new Claim("FullName",user.FirstName+" "+user.LastName),
            new Claim(ClaimTypes.Name,user.Username),
            new Claim(ClaimTypes.Role,user.Role.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        return Redirect("/Home");
    }
    [HttpGet("Logout")]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
    }
	//[Authorize(Roles = "Admin")]
	//[HttpGet("Test")]
	//public string TestingAuth()
	//{
	//	return "Success!";
	//}
}
