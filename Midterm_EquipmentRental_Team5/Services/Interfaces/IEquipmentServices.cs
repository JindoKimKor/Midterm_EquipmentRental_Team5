using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface IEquipmentServices
    {
        Task<IEnumerable<IEquipment>> GetAllEquipmentAsync(int page = 1);
        Task<IEquipment> GetEquipmentByIdAsync(int id);
        Task AddEquipmentAsync(IEquipment newEquipment);
        Task UpdateEquipmentAsync(int id, IEquipment updatedEquipment);
        Task DeleteEquipmentAsync(int id);
        Task<IEnumerable<IEquipment>> GetAvailableEquipmentAsync();
        Task<IEnumerable<IEquipment>> GetRentedEquipmentAsync();

    }
}