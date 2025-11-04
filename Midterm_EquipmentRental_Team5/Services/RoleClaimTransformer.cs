using Microsoft.AspNetCore.Authentication;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;
using System.Security.Claims;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class RoleClaimsTransformer : IClaimsTransformation
    {
        private readonly IConfiguration _cfg;
        private readonly IAppUserRepository _appUserRepository;
        public RoleClaimsTransformer(IConfiguration cfg, IAppUserRepository appUserRepository)
        {
            _cfg = cfg;
            _appUserRepository = appUserRepository;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identities.FirstOrDefault(i => i.IsAuthenticated);
            if (identity is null)
                return Task.FromResult(principal);

            if (identity.HasClaim(c => c.Type == ClaimTypes.Role))
                return Task.FromResult(principal);

            var email = principal.FindFirstValue(ClaimTypes.Email);

            var role = "User";
            if (!string.IsNullOrWhiteSpace(email))
            {
                var appUser = _appUserRepository.GetByEmailAsync(email).Result;
                if (appUser != null)
                {
                    role = appUser.Role;
                }
            }

            // Crucial: use ClaimTypes.Role; the API will read this from the JWT
            identity.AddClaim(new Claim(ClaimTypes.Role, role));

            return Task.FromResult(principal);
        }
    }
}
