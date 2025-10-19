using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface IEquipmentServices
    {
        IEnumerable<IEquipment>? GetAllEquipmentAsync(int page = 1);
        IEquipment GetEquipmentByIdAsync(int id);
        void AddEquipmentAsync(IEquipment newEquipment);
        IEquipment? UpdateEquipmentAsync(int id, IEquipment updatedEquipment);
        void DeleteEquipmentAsync(int id);
        IEnumerable<IEquipment>? GetAvailableEquipmentAsync();
        IEnumerable<IEquipment>? GetRentedEquipmentAsync();

    }
}