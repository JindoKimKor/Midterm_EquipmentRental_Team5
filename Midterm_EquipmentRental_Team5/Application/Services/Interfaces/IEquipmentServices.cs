using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Application.Interfaces
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