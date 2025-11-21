namespace Midterm_EquipmentRental_Team5.Domain.Interfaces
{
    public interface ILoginRequest
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}