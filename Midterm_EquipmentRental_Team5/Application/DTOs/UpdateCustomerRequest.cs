using System.ComponentModel.DataAnnotations;

namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    /// <summary>
    /// DTO for updating customer information
    /// </summary>
    public class UpdateCustomerRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }

        public string? State { get; set; }

        /// <summary>
        /// Only Admins can change this field
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Only Admins can change this field
        /// </summary>
        public bool Active { get; set; }
    }
}
