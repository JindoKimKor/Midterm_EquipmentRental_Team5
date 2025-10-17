using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Models.DTOs
{
    public class ExtensionRequest : IExtensionRequest
    {
        public DateTime NewDueDate { get; set; }
        public string Reason { get; set; }
    }
}