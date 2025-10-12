using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly AppDbContext _context;

        public EquipmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public Equipment AddNewEquipment(Equipment equipment)
        {
            throw new NotImplementedException();
        }

        public void DeleteEquipment(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Equipment> GetAllEquipment()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Equipment> GetRentedEquipment()
        {
            throw new NotImplementedException();
        }

        public Equipment? GetSpecificEquipment(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Equipment> ListAvailableEquipment()
        {
            throw new NotImplementedException();
        }

        public void UpdateEquipment(Equipment equipment)
        {
            throw new NotImplementedException();
        }
    }
}
