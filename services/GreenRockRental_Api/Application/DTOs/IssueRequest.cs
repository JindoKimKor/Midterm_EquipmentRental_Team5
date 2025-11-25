using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Application.DTOs
{
    public class IssueRequest : IIssueRequest
    {
        public int EquipmentId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? DueDate { get; set; } // Optional, defaults to 7 days if null
    }
}