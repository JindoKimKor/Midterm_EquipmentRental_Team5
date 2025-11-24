using Midterm_EquipmentRental_Team5.Infrastructure.Persistence;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _context;

        public RentalRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateRental(IRental rental)
        {
            _context.Rentals.Add((Rental)rental);
        }

        public void UpdateRental(IRental rental)
        {
            _context.Rentals.Update((Rental)rental);
        }

        public async Task<IEnumerable<IRental>> GetAllRentals()
        {
            return await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToListAsync();
        }

        public async Task<IRental?> GetRentalDetails(int id)
        {
            return await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public void IssueEquipment(IRental rental, DateTime dueDate)
        {
            rental.IssuedAt = DateTime.UtcNow;
            rental.DueDate = dueDate;
            rental.IsActive = true;
            _context.Rentals.Add((Rental)rental);
        }

        public async Task ReturnEquipment(int id)
        {
            var rental = await _context.Rentals.FindAsync(id) ?? throw new KeyNotFoundException();
            rental.ReturnedAt = DateTime.UtcNow;
            rental.IsActive = false;
            _context.Rentals.Update(rental);
        }

        public async Task CancelRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id) ?? throw new KeyNotFoundException();
            _context.Rentals.Remove(rental);
        }

        public async Task ExtendRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id) ?? throw new KeyNotFoundException();
            rental.DueDate = rental.DueDate.AddDays(7);
            _context.Rentals.Update(rental);
        }

        public async Task<IEnumerable<IRental>> GetActiveRentals()
        {
            return await _context.Rentals
                .Where(r => r.IsActive)
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToListAsync();
        }

        public async Task<IEnumerable<IRental>> GetCompletedRentals()
        {
            // Completed: EndDate not null and not cancelled
            return await _context.Rentals
                .Where(r => !r.IsActive && r.ReturnedAt != null)
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToListAsync();
        }

        public async Task<IEnumerable<IRental>> GetEquipmentRentalHistory(int equipmentId)
        {
            return await _context.Rentals
                .Where(r => r.EquipmentId == equipmentId)
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToListAsync();
        }

        public async Task<IEnumerable<IRental>> GetOverdueRentals()
        {
            // Active rentals where DueDate < Now and not yet returned
            return await _context.Rentals
                 .Where(r => r.IsActive && r.DueDate < DateTime.UtcNow && r.ReturnedAt == null)
                 .Include(r => r.Customer)
                 .Include(r => r.Equipment)
                 .OrderBy(r => r.DueDate)
                 .ToListAsync();
        }
    }
}