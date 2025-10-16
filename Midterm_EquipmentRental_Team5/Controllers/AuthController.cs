using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Midterm_EquipmentRental_Team5.Models.DTOs;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Services.Interfaces;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtSettings _jwtSettings; // Inject JwtSettings

        public AuthController(IAuthService authService, JwtSettings jwtSettings)
        {
            _authService = authService;
            _jwtSettings = jwtSettings; // Store injected settings
        }

        // POST /api/auth/login - Authenticate user & return JWT token with role claims
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Validate credentials using AuthService
            var customer = _authService.ValidateLogin(request.Username, request.Password);

            if (customer == null)
            {
                return Unauthorized("Invalid username or password"); // Return 401 if credentials don't match
            }

            // Generate JWT token for authenticated customer
            var token = GenerateJwtToken(customer);

            // Return token in response
            return Ok(new LoginResponse { Token = token });
        }

        private string GenerateJwtToken(Customer customer)
        {
            // Create claims with customer info
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, customer.UserName),
                new Claim(ClaimTypes.Role, customer.Role),
                new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString())
            };

            // Get secret key from injected JwtSettings
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create token with expiration from settings
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );

            // Return serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}