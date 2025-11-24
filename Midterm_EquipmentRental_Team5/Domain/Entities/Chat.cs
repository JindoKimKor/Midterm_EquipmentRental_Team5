using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midterm_EquipmentRental_Team5.Domain.Entities
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        [Required]
        public int SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public Customer? Sender { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public Customer? Receiver { get; set; }
    }
}