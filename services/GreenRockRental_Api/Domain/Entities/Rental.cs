using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Domain.Entities
{
    public class Rental : IRental
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int EquipmentId { get; set; }

        [ForeignKey(nameof(EquipmentId))]
        public Equipment? Equipment { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public bool IsActive { get; set; }
        public decimal? OverdueFee { get; set; }
        public string? ExtensionReason { get; set; }
    }
}
