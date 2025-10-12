using Midterm_EquipmentRental_Team5.Models;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface IAuthService
    {
        Customer? ValidateLogin(string username, string password);
    }
}
