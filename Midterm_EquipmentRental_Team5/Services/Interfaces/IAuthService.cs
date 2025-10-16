using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface IAuthService
    {
        IUser? ValidateLogin(ILoginRequest loginRequest);
        object GenerateJwtToken(IUser user);
    }
}
