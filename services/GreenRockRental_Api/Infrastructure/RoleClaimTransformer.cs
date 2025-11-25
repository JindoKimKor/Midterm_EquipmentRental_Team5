using Microsoft.AspNetCore.Authentication;
using GreenRockRental_Api.Infrastructure.Repositories.Interfaces;
using System.Security.Claims;
using GreenRockRental_Api.Infrastructure.Repositories;

namespace GreenRockRental_Api.Infrastructure
{
    public class RoleClaimsTransformer : IClaimsTransformation
    {
        private readonly IConfiguration _cfg;
        private readonly ICustomerRepository _customerRepository;

        public RoleClaimsTransformer(IConfiguration cfg, ICustomerRepository customerRepository)
        {
            _cfg = cfg;
            _customerRepository = customerRepository;
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
            var customer = await _customerRepository.GetCustomerByEmail(email);
            if (customer == null)
                return principal;

            // --- Modify or Add NameIdentifier ---
            var existingIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            if (existingIdClaim != null)
            {
                // Replace the existing NameIdentifier
                identity.RemoveClaim(existingIdClaim);
            }

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()));

            // --- Modify or Add Role ---
            var existingRoleClaim = identity.FindFirst(ClaimTypes.Role);
            if (existingRoleClaim != null)
            {
                identity.RemoveClaim(existingRoleClaim);
            }

            identity.AddClaim(new Claim(ClaimTypes.Role, customer.Role ?? "User"));

            return principal;
        }
    }
}
