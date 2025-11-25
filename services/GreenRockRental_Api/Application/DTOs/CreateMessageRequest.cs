using System.ComponentModel.DataAnnotations;

namespace GreenRockRental_Api.Application.DTOs
{
    /// <summary>
    /// DTO for creating a new message
    /// </summary>
    public class CreateMessageRequest
    {
        [Required]
        public int ReceiverId { get; set; }

        [Required]
        public string? Content { get; set; }

        [Required]
        public int ChatId { get; set; }
    }
}
