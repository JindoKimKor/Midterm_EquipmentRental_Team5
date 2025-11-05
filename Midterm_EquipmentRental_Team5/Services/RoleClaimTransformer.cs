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

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identities.FirstOrDefault(i => i.IsAuthenticated);
            if (identity is null)
                return principal;

            var email = principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
                return principal;

            // Get user info from your repository
            var appUser = await _appUserRepository.GetByEmailAsync(email);
            if (appUser == null)
                return principal;

            // --- Modify or Add NameIdentifier ---
            var existingIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            if (existingIdClaim != null)
            {
                // Replace the existing NameIdentifier
                identity.RemoveClaim(existingIdClaim);
            }

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()));

            // --- Modify or Add Role ---
            var existingRoleClaim = identity.FindFirst(ClaimTypes.Role);
            if (existingRoleClaim != null)
            {
                identity.RemoveClaim(existingRoleClaim);
            }

            identity.AddClaim(new Claim(ClaimTypes.Role, appUser.Role ?? "User"));

            return principal;
        }
    }
}
