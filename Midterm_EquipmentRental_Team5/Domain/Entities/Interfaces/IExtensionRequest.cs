namespace Midterm_EquipmentRental_Team5.Domain.Interfaces
{
    public interface IExtensionRequest
    {
        DateTime NewDueDate { get; set; }
        string Reason { get; set; }
    }
}