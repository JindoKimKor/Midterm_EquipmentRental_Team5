namespace Midterm_EquipmentRental_Team5.Models.DTOs
{
    public class IssueRequest
    {
        public int EquipmentId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? DueDate { get; set; } // Optional, defaults to 7 days if null
    }
}