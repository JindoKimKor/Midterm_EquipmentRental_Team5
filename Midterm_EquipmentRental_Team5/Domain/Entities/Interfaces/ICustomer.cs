namespace Midterm_EquipmentRental_Team5.Domain.Interfaces
{
    public interface ICustomer
    {
        int Id { get; set; }
        string? Name { get; set; }
        string? Email { get; set; }
        string? UserName { get; set; }
        string? Password { get; set; }
        string? Role { get; set; }
        string? ExternalProvider { get; set; }
        string? ExternalId { get; set; }
    }
}