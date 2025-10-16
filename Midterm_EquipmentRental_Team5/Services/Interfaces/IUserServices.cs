
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface IUserServices
    {
        IUser GetUser(ILoginRequest user);
    }
}