using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Services.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class AuthService : IAuthService
    {
        public Customer? ValidateLogin(string username, string password)
        {
            throw new NotImplementedException();
        }

        private object GenerateJwtToken(User user)
        {
            var claims = new[]{
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("a3f81b9e47d84526b9c2eab2fdcfed48"));
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
