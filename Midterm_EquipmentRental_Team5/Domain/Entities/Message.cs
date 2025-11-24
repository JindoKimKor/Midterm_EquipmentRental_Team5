using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midterm_EquipmentRental_Team5.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public int SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public Customer? Sender { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public Customer? Receiver { get; set; }

        [Required]
        public required string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        [Required]
        public int ChatId { get; set; }
    }
}