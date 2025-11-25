using GreenRockRental_Api.Domain.Entities;
using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Infrastructure.Repositories.Interfaces
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
