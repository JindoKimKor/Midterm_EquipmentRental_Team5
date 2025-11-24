using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ICustomer?> ValidateLogin(ILoginRequest loginRequest);
        object GenerateJwtToken(ICustomer user);
    }
}
