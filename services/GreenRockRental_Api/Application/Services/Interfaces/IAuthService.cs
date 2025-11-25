using GreenRockRental_Api.Domain.Entities;
using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ICustomer?> ValidateLogin(ILoginRequest loginRequest);
        string GenerateJwtToken(ICustomer user);
    }
}
