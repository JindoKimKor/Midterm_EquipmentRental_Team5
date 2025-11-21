using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;
using Midterm_EquipmentRental_Team5.Application.Interfaces;
using Midterm_EquipmentRental_Team5.Infrastructure.UnitOfWork;

namespace Midterm_EquipmentRental_Team5.Application.Services
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

        public ICustomer? ValidateLogin(ILoginRequest loginRequest)
        {
            var user = _unitOfWork.Customers.GetCustomerByPasswordAndUsername(loginRequest) ?? null;
            return user;
        }

        public object GenerateJwtToken(ICustomer customer)
        {
            var claims = new[]{
                new Claim(ClaimTypes.Name, customer.UserName),
                new Claim(ClaimTypes.Role, customer.Role),
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