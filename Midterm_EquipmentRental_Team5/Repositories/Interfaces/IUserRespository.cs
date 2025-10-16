using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface IUserRespository
    {
        IUser GetUserByPasswordAndUserName(string password, string userName);
    }
}
