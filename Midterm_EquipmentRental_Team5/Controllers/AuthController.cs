using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Services.Interfaces;
using Midterm_EquipmentRental_Team5.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, JwtSettings jwtSettings)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = _authService.ValidateLogin(request) ?? throw new KeyNotFoundException("User not found");
                var token = _authService.GenerateJwtToken(user);
                return Ok(new { token, user });
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
        public IActionResult Denied() => Content("Access denied.");
    }
}