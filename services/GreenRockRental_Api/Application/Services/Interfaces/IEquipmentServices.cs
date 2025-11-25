using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Application.Interfaces
{
    public interface IEquipmentServices
    {
        Task<IEnumerable<IEquipment>?> GetAllEquipment(int page = 1);
        Task<IEquipment?> GetEquipmentById(int id);
        Task AddEquipment(IEquipment newEquipment);
        Task UpdateEquipment(int id, IEquipment updatedEquipment);
        Task DeleteEquipment(int id);
        Task<IEnumerable<IEquipment>?> GetAvailableEquipment();
        Task<IEnumerable<IEquipment>?> GetRentedEquipment();

    }
}