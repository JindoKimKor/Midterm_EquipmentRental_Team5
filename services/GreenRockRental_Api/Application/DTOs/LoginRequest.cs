using Microsoft.AspNetCore.Identity.Data;
using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Application.DTOs
{
    public class LoginRequest : ILoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
