using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface IEquipmentRepository
    {
        IEnumerable<IEquipment> GetAllEquipment();
        IEquipment? GetSpecificEquipment(int id);
        IEquipment AddNewEquipment(IEquipment equipment);
        void UpdateEquipment(IEquipment equipment);
        void DeleteEquipment(int id);
        IEnumerable<IEquipment> ListAvailableEquipment();
        IEnumerable<IEquipment> GetRentedEquipment();
    }
}
