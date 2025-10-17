using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;
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

        public IEquipment AddNewEquipment(IEquipment equipment)
        {
            _context.Equipment.Add((Equipment)equipment);
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

        public IEnumerable<IEquipment> GetAllEquipment()
        {
            return _context.Equipment.ToList();
        }

        public IEnumerable<IEquipment> GetRentedEquipment()
        {
            return _context.Equipment.Where(e => !e.IsAvailable).ToList();
        }

        public IEquipment? GetSpecificEquipment(int id)
        {
            return _context.Equipment.Find(id);
        }

        public IEnumerable<IEquipment> ListAvailableEquipment()
        {
            return _context.Equipment.Where(e => e.IsAvailable).ToList();
        }

        public void UpdateEquipment(IEquipment equipment)
        {
            var existingEquipment = _context.Equipment.Find(equipment.Id);
            if (existingEquipment != null)
            {
                _context.Entry(existingEquipment).CurrentValues.SetValues(equipment);
            }
        }
    }
}