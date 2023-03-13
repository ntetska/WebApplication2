using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AdeiesApplication.Persistance;
using AdeiesApplication.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using AdeiesApplication.Resources;

namespace AdeiesApplication.Controllers.Api;
[Route("backend_api/[controller]")]
[ApiController]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<UserCreate> _passwordHasher;
    private readonly IStringLocalizer<Localizer> _sharedResourceLocalizer;

    public AuthController(UserRepository userRepository, PasswordHasher<UserCreate> passwordHasher, IStringLocalizer<Localizer> stringLocalizer)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _sharedResourceLocalizer = stringLocalizer;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
    {

        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            return Unauthorized(_sharedResourceLocalizer["WrongData"].Value);
        }
        var userCreate = user.ToCreate();
        var result = _passwordHasher.VerifyHashedPassword(userCreate, userCreate.Password, password);
        //check user's password & activity
        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(_sharedResourceLocalizer["Unauthorized"].Value);
        }
        if (user.IsActive == false)
        {
            return Unauthorized(_sharedResourceLocalizer["Unauthorized"].Value);
        }
        var claims = new List<Claim>
        {
            new Claim("FullName",user.FirstName+" "+user.LastName),
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name,user.Username),
            new Claim(ClaimTypes.Role,user.Role.ToString()),
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
        return Ok();
    }

    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok();
    }

    [HttpGet("AccessDenied")]
    public IActionResult AccessDenied()
    {
        return Forbid(_sharedResourceLocalizer["notAccess"].Value);
    }
}
