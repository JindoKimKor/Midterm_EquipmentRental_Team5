namespace Midterm_EquipmentRental_Team5.Domain.Interfaces
{
    public interface IAppUser
    {
        int Id { get; set; }
        string Email { get; set; }
        string Role { get; set; }
        string? ExternalProvider { get; set; }
        string? ExternalId { get; set; }
    }
}
