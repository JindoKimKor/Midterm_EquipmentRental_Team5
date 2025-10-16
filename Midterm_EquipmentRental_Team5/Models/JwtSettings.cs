using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Models
{
    public class JwtSettings : IJwtSettings
    {
        // JWT Token configs
        public string SecretKey { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 30;
    }
}
