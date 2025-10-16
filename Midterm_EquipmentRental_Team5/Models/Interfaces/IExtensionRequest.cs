namespace Midterm_EquipmentRental_Team5.Models.Interfaces
{
    public interface IExtensionRequest
    {
        DateTime NewDueDate { get; set; }
        string Reason { get; set; }
    }
}