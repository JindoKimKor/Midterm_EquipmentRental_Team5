using System.ComponentModel.DataAnnotations;

namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    /// <summary>
    /// DTO for creating a new customer
    /// </summary>
    public class CreateCustomerRequest
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }

        public string? State { get; set; }

        public string? Role { get; set; } = "User";
    }
}
