using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Application.DTOs
{
    public class ExtensionRequest : IExtensionRequest
    {
        public DateTime NewDueDate { get; set; }
        public string? Reason { get; set; }
    }
}