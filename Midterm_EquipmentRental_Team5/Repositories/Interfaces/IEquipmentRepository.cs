using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface IEquipmentRepository
    {
        IEquipment AddNewEquipment(IEquipment equipment);
        IEquipment? DeleteEquipment(int id);
        IEnumerable<IEquipment>? GetAllEquipment();
        IEnumerable<IEquipment>? GetRentedEquipment();
        IEquipment? GetSpecificEquipment(int id);
        IEnumerable<IEquipment>? ListAvailableEquipment();
        IEquipment? UpdateEquipment(IEquipment equipment);
    }
}
