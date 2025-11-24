using System.ComponentModel.DataAnnotations;

namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    /// <summary>
    /// DTO for creating new equipment
    /// </summary>
    public class CreateEquipmentRequest
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        public string? Condition { get; set; }

        [Required]
        public decimal RentalPrice { get; set; }

        public string? ImageUrl { get; set; }
    }
}
