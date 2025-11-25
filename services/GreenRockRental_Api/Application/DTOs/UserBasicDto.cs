namespace GreenRockRental_Api.Application.DTOs
{
    /// <summary>
    /// DTO for basic user information - safe to expose in API responses
    /// </summary>
    public class UserBasicDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
