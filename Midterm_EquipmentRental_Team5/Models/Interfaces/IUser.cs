namespace Midterm_EquipmentRental_Team5.Models.Interfaces
{
    public interface IUser
    {
        int Id { get; set; }
        string HashedPassword { get; set; }
        string UserName { get; set; }
        string Role { get; set; }
    }
}