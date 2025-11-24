using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team5.Infrastructure.Persistence;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;
using Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories
{
    public class EquipmentRepository(AppDbContext context) : IEquipmentRepository
    {
        private readonly AppDbContext _context = context;

        public void AddNewEquipment(IEquipment equipment)
        {
            _context.Equipment.Add((Equipment)equipment);
        }

        public async Task DeleteEquipment(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment != null) _context.Equipment.Remove(equipment);
        }

        public async Task<IEnumerable<IEquipment>> GetAllEquipment()
        {
            return await _context.Equipment.ToListAsync();
        }

        public async Task<IEnumerable<IEquipment>> GetRentedEquipment()
        {
            return await _context.Equipment.Where(e => !e.IsAvailable).ToListAsync();
        }

        public async Task<IEquipment?> GetSpecificEquipment(int id)
        {
            var equipment = await _context.Equipment.FirstOrDefaultAsync(e => e.Id == id);
            return equipment ?? null;
        }

        public async Task<IEnumerable<IEquipment>> ListAvailableEquipment()
        {
            return await _context.Equipment.Where(e => e.IsAvailable).ToListAsync();
        }

        public async Task UpdateEquipment(IEquipment equipment)
        {
            var existingEquipment = await _context.Equipment.FindAsync(equipment.Id);
            if (existingEquipment != null) _context.Entry(existingEquipment).CurrentValues.SetValues(equipment);
        }
    }
}