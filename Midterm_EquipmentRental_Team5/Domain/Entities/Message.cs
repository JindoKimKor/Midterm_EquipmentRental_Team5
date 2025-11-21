using System.ComponentModel.DataAnnotations;

namespace Midterm_EquipmentRental_Team5.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        [Required]
        public int ChatId { get; set; }
    }
}