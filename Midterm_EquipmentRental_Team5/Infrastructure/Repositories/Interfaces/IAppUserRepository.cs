using Midterm_EquipmentRental_Team5.Domain.Entities;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces
{
    public interface IAppUserRepository
    {
        Task<AppUser?> GetByEmailAsync(string email);
    }
}
