namespace GreenRockRental_Api.Application.DTOs
{
    /// <summary>
    /// DTO for returning customer list items - minimal customer information
    /// </summary>
    public class CustomerListDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public bool Active { get; set; }
    }
}
