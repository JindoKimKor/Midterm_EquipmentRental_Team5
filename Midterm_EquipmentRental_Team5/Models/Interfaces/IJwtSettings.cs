namespace Midterm_EquipmentRental_Team5.Models.Interfaces
{
    public interface IJwtSettings
    {
        string SecretKey { get; set; }
        int ExpirationMinutes { get; set; }
    }
}