using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface IEquipmentServices
    {
        IEnumerable<IEquipment>? GetAllEquipment(int page = 1);
        IEquipment GetEquipmentById(int id);
        void AddEquipment(IEquipment newEquipment);
        void UpdateEquipment(int id, IEquipment updatedEquipment);
        void DeleteEquipment(int id);
        IEnumerable<IEquipment>? GetAvailableEquipment();
        IEnumerable<IEquipment>? GetRentedEquipment();

    }
}