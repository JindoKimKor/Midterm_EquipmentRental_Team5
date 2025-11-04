using Midterm_EquipmentRental_Team5.Models;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface IAppUserRepository
    {
        Task<AppUser?> GetByEmailAsync(string email);
    }
}
