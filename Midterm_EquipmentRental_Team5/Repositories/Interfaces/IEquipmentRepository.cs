using Midterm_EquipmentRental_Team5.Models;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface IEquipmentRepository
    {
        IEnumerable<Equipment> GetAllEquipment();
        Equipment? GetSpecificEquipment(int id);
        Equipment AddNewEquipment(Equipment equipment);
        void UpdateEquipment(Equipment equipment);
        void DeleteEquipment(int id);
        IEnumerable<Equipment> ListAvailableEquipment();
        IEnumerable<Equipment> GetRentedEquipment();
    }
}
