namespace Midterm_EquipmentRental_Team5.Models.DTOs
{
    public class ReturnRequest
    {
        public int RentalId { get; set; }
        public string Condition { get; set; } // Equipment condition after return
        public string? Notes { get; set; }
    }
}