using Microsoft.AspNetCore.Identity.Data;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Models.DTOs
{
    public class LoginRequest : ILoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
