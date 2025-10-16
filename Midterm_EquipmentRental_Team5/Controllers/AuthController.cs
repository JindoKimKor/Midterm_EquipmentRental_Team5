using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Models.DTOs;
using Midterm_EquipmentRental_Team5.Services.Interfaces;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserServices _userServices;

        public AuthController(IAuthService authService, IUserServices userServices)
        {
            _authService = authService;
            _userServices = userServices;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = _userServices.(request);
                var token = GenerateJwtToken(user);
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
