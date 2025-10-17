namespace Midterm_EquipmentRental_Team5.Models.Interfaces
{
    public interface IEquipment
    {
        int Id { get; set; }
        string? Name { get; set; }
        string? Description { get; set; }
        string? Category { get; set; }
        string? Condition { get; set; }
        decimal RentalPrice { get; set; }
        bool IsAvailable { get; set; }
        DateTime CreatedAt { get; set; }
    }
}