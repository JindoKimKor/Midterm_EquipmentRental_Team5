using System.ComponentModel.DataAnnotations;

namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    /// <summary>
    /// DTO for updating equipment
    /// </summary>
    public class UpdateEquipmentRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Condition { get; set; }

        [Required]
        public decimal RentalPrice { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        public string ImageUrl { get; set; }
    }
}
