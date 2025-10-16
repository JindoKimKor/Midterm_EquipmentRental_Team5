namespace Midterm_EquipmentRental_Team5.Models.Interfaces
{
    public interface ICustomer
    {
        int Id { get; set; }
        string? Name { get; set; }
        string? Email { get; set; }
        string? UserName { get; set; }
        string? Password { get; set; }
        string? Role { get; set; }
    }
}