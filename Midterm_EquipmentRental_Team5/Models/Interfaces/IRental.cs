namespace Midterm_EquipmentRental_Team5.Models.Interfaces
{
    public interface IRental
    {
        int Id { get; set; }
        int CustomerId { get; set; }
        int EquipmentId { get; set; }
        Equipment Equipment { get; set; }
        Customer Customer { get; set; }
        DateTime IssuedAt { get; set; }
        DateTime DueDate { get; set; }
        DateTime? ReturnedAt { get; set; }
        bool IsActive { get; set; }
        decimal? OverdueFee { get; set; }
        string? ExtensionReason { get; set; }
    }
}