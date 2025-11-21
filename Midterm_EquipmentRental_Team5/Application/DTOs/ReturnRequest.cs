using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    public class ReturnRequest : IReturnRequest
    {
        public int RentalId { get; set; }
        public string Condition { get; set; } // Equipment condition after return
        public string? Notes { get; set; }
    }
}