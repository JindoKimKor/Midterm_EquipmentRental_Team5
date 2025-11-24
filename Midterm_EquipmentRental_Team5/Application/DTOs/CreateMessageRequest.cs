using System.ComponentModel.DataAnnotations;

namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    /// <summary>
    /// DTO for creating a new message
    /// </summary>
    public class CreateMessageRequest
    {
        [Required]
        public int ReceiverId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int ChatId { get; set; }
    }
}
