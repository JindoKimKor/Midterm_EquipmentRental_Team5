namespace Midterm_EquipmentRental_Team5.Models.Interfaces
{
    public interface ILoginRequest
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}