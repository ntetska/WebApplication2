using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
    {
        var user = _context.User.FirstOrDefault(s => s.Username == username);

        if (user.Password != password)
        {
            return BadRequest();
        }

        if(user.IsActive == false)
        {
            return BadRequest();
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
}
