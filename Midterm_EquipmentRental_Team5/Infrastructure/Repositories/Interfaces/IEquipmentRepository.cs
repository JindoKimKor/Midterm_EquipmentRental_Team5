using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces
{
    public interface IEquipmentRepository
    {
        void AddNewEquipment(IEquipment equipment);
        Task DeleteEquipment(int id);
        Task<IEnumerable<IEquipment>> GetAllEquipment();
        Task<IEnumerable<IEquipment>> GetRentedEquipment();
        Task<IEquipment?> GetSpecificEquipment(int id);
        Task<IEnumerable<IEquipment>> ListAvailableEquipment();
        Task UpdateEquipment(IEquipment equipment);
    }
}
