using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Application.Interfaces;
using Midterm_EquipmentRental_Team5.Application.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;

namespace Midterm_EquipmentRental_Team5.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _authService.ValidateLogin(request) ?? throw new KeyNotFoundException("User not found");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName ?? "User"),
                    new Claim(ClaimTypes.Role, user.Role ?? "User")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };

                // This generates the cookie and sets it in the response cookie header
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                var token = _authService.GenerateJwtToken(user);

                var response = new LoginResponseDto
                {
                    Message = "Login successful",
                    User = new UserBasicDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Role = user.Role
                    },
                    Token = token
                };

                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return Unauthorized("Invalid username and password");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem("An error occurred while retrieving by id.");
            }
        }

        // Start Google login
        [HttpGet("google-login")]
        public IActionResult GoogleLogin(string? returnUrl = "http://localhost:5173")
        {
            var props = new AuthenticationProperties { RedirectUri = returnUrl };
            props.SetParameter("prompt", "select_account");
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        // Optional logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return Forbid(); // returns 403 Forbidden
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                Id = userId,
                UserName = userName,
                Role = role
            });
        }

        [HttpGet("authorized")]
        [Authorize]
        public IActionResult IsUserAuthorized()
        {
            return Ok(new { message = "User is authorized" });
        }
    }
}