using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    public class ExtensionRequest : IExtensionRequest
    {
        public DateTime NewDueDate { get; set; }
        public string? Reason { get; set; }
    }
}