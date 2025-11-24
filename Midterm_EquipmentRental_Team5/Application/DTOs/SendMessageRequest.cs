using System.ComponentModel.DataAnnotations;

namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    public class SendMessageDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; } = string.Empty;

        [Required]
        public string ReceiverId { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        [Required]
        public int ChatId { get; set; }
    }
}
