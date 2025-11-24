namespace Midterm_EquipmentRental_Team5.Domain.Interfaces
{
    public interface ICustomer
    {
        int Id { get; set; }
        string? Name { get; set; }
        string? Email { get; set; }
        string? UserName { get; set; }
        string? Password { get; set; }
        string? Phone { get; set; }
        string? Address { get; set; }
        string? City { get; set; }
        string? ZipCode { get; set; }
        string? State { get; set; }
        string? Role { get; set; }
        bool Active { get; set; }
        DateTime CreatedAt { get; set; }
        string? ExternalProvider { get; set; }
        string? ExternalId { get; set; }
    }
}
