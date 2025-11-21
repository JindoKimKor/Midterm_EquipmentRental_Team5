using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midterm_EquipmentRental_Team5.Domain.Entities
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = new();

    }
}