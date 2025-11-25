namespace GreenRockRental_Api.Domain.Interfaces
{
    public interface IJwtSettings
    {
        string SecretKey { get; set; }
        int ExpirationMinutes { get; set; }
    }
}