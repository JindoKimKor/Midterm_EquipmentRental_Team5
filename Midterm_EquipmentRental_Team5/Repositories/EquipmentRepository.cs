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
            _context.Equipment.Add(equipment);
            return equipment;
        }

        public void DeleteEquipment(int id)
        {
            var equipment = _context.Equipment.Find(id);
            if (equipment != null)
            {
                _context.Equipment.Remove(equipment);
            }
        }

        public IEnumerable<Equipment> GetAllEquipment()
        {
            return _context.Equipment.ToList();
        }

        public IEnumerable<Equipment> GetRentedEquipment()
        {
            return _context.Equipment.Where(e => !e.IsAvailable).ToList();
        }

        public Equipment? GetSpecificEquipment(int id)
        {
            return _context.Equipment.Find(id);
        }

        public IEnumerable<Equipment> ListAvailableEquipment()
        {
            return _context.Equipment.Where(e => e.IsAvailable).ToList();
        }

        public void UpdateEquipment(Equipment equipment)
        {
            var existingEquipment = _context.Equipment.Find(equipment.Id);
            if (existingEquipment != null)
            {
                _context.Entry(existingEquipment).CurrentValues.SetValues(equipment);
            }
        }
    }
}
