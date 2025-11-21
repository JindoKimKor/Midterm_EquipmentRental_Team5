using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces
{
    public interface IEquipmentRepository
    {
        void AddNewEquipment(IEquipment equipment);
        void DeleteEquipment(int id);
        IEnumerable<IEquipment> GetAllEquipment();
        IEnumerable<IEquipment> GetRentedEquipment();
        IEquipment? GetSpecificEquipment(int id);
        IEnumerable<IEquipment> ListAvailableEquipment();
        void UpdateEquipment(IEquipment equipment);
    }
}
