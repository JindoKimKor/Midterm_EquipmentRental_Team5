using System.ComponentModel.DataAnnotations;

namespace GreenRockRental_Api.Application.DTOs
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
