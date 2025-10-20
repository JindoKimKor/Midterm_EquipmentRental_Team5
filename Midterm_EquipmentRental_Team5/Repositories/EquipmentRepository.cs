using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories
{
    public class EquipmentRepository(AppDbContext context) : IEquipmentRepository
    {
        private readonly AppDbContext _context = context;

        public void AddNewEquipment(IEquipment equipment)
        {
            _context.Equipment.Add((Equipment)equipment);
        }

        public void DeleteEquipment(int id)
        {
            var equipment = _context.Equipment.Find(id);
            if (equipment != null) _context.Equipment.Remove(equipment);
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
            var equipment = _context.Equipment.FirstOrDefault(e => e.Id == id);
            return equipment ?? null;
        }

        public IEnumerable<IEquipment> ListAvailableEquipment()
        {
            return _context.Equipment.Where(e => e.IsAvailable).ToList();
        }

        public void UpdateEquipment(IEquipment equipment)
        {
            var existingEquipment = _context.Equipment.Find(equipment.Id);
            if (existingEquipment != null) _context.Entry(existingEquipment).CurrentValues.SetValues(equipment);
        }
    }
}