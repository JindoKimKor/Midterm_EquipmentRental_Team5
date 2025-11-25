using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using GreenRockRental_Api.Domain.Entities;
using GreenRockRental_Api.Domain.Interfaces;
using GreenRockRental_Api.Application.Interfaces;
using GreenRockRental_Api.Infrastructure.UnitOfWork;

namespace GreenRockRental_Api.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUnitOfWork unitOfWork, JwtSettings jwtSettings)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
        }

        public async Task<ICustomer?> ValidateLogin(ILoginRequest loginRequest)
        {
            var user = await _unitOfWork.Customers.GetCustomerByPasswordAndUsername(loginRequest) ?? null;
            return user;
        }

        public string GenerateJwtToken(ICustomer customer)
        {
            var claims = new[]{
                new Claim(ClaimTypes.Name, customer.UserName ?? string.Empty),
                new Claim(ClaimTypes.Role, customer.Role ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                // issuer: "domain",
                // audience: "domain",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
