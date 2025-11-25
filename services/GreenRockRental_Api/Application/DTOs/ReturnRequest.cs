using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Application.DTOs
{
    public class ReturnRequest : IReturnRequest
    {
        public int RentalId { get; set; }
        public string? Condition { get; set; } // Equipment condition after return
        public string? Notes { get; set; }
    }
}