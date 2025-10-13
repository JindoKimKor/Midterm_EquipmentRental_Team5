using Microsoft.AspNetCore.Http;
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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // TODO: POST /api/auth/login - Authenticate user & return JWT token with role claims
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
