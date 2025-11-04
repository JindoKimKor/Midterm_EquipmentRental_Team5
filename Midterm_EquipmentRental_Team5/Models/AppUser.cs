using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Models
{
    public class AppUser : IAppUser
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Role { get; set; } = "User"; // "Admin" or "User" 
        public string? ExternalProvider { get; set; } = "Google";
        public string? ExternalId { get; set; }
    }
}