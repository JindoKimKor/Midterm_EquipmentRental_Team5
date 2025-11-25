using GreenRockRental_Api.Infrastructure.Persistence;
using GreenRockRental_Api.Domain.Entities;
using GreenRockRental_Api.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Infrastructure.Repositories
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
            var rentalEntity = rental as Rental ?? throw new InvalidCastException("rental must be of type Rental");
            _context.Rentals.Add(rentalEntity);
        }

        public void UpdateRental(IRental rental)
        {
            var rentalEntity = rental as Rental ?? throw new InvalidCastException("rental must be of type Rental");
            _context.Rentals.Update(rentalEntity);
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
            var rentalEntity = rental as Rental ?? throw new InvalidCastException("rental must be of type Rental");
            _context.Rentals.Add(rentalEntity);
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