using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace GreenRockRental_Api.Domain.Entities
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            // Return ASP.NET Identity User.Id
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
