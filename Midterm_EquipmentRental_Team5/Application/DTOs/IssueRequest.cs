using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    public class IssueRequest : IIssueRequest
    {
        public int EquipmentId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? DueDate { get; set; } // Optional, defaults to 7 days if null
    }
}