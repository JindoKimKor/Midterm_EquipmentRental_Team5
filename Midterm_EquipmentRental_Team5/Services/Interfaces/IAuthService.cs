using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface IAuthService
    {
        ICustomer? ValidateLogin(ILoginRequest loginRequest);
        object GenerateJwtToken(ICustomer user);
    }
}
