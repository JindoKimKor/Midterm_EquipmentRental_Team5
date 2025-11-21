namespace Midterm_EquipmentRental_Team5.Domain.Interfaces
{
    public interface IJwtSettings
    {
        string SecretKey { get; set; }
        int ExpirationMinutes { get; set; }
    }
}