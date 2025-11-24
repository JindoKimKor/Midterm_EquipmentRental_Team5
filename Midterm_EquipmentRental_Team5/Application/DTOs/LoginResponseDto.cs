namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    /// <summary>
    /// DTO for login response - returns user info and JWT token without exposing password
    /// </summary>
    public class LoginResponseDto
    {
        public string Message { get; set; }
        public UserBasicDto User { get; set; }
        public string Token { get; set; }
    }
}
