using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Services.Interfaces;
using Midterm_EquipmentRental_Team5.Models.DTOs;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

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
                return Ok(new { Token = token });
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
    }
}