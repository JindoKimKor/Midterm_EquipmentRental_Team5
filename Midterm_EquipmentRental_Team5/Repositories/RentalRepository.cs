using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _context;

        public RentalRepository(AppDbContext context)
        {
            _context = context;
        }

        public IRental CreateRental(IRental rental)
        {
            _context.Rentals.Add((Rental)rental);
            return rental;
        }

        public void UpdateRental(IRental rental)
        {
            _context.Rentals.Update((Rental)rental);
        }

        public IEnumerable<IRental>? GetAllRentals()
        {
            var rentals = _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToList();

            return rentals.Count != 0 ? rentals : null;
        }

        public IRental? GetRentalDetails(int id)
        {
            return _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .FirstOrDefault(r => r.Id == id);
        }

        public IRental IssueEquipment(IRental rental, DateTime dueDate)
        {
            rental.IssuedAt = DateTime.UtcNow;
            rental.DueDate = dueDate;
            rental.IsActive = true;
            _context.Rentals.Add((Rental)rental);
            return rental;
        }

        public IRental? ReturnEquipment(int id)
        {
            var rental = _context.Rentals.Find(id);
            if (rental == null) return null;

            rental.ReturnedAt = DateTime.UtcNow;
            rental.IsActive = false;
            _context.Rentals.Update(rental);
            return rental;
        }

        public void CancelRental(int id)
        {
            var rental = _context.Rentals.Find(id);
            if (rental == null) return;
            _context.Rentals.Remove(rental);
        }

        public void ExtendRental(int id)
        {
            var rental = _context.Rentals.Find(id);
            if (rental == null) return;

            rental.DueDate = rental.DueDate.AddDays(7);
            _context.Rentals.Update(rental);
        }

        public IEnumerable<IRental>? GetActiveRentals()
        {
            var rentals = _context.Rentals
                .Where(r => r.IsActive)
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToList();

            return rentals.Count != 0 ? rentals : null;
        }

        public IEnumerable<IRental>? GetCompletedRentals()
        {
            // Completed: EndDate not null and not cancelled
            var rentals = _context.Rentals
                .Where(r => !r.IsActive && r.ReturnedAt != null)
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToList();

            return rentals.Count != 0 ? rentals : null;
        }

        public IEnumerable<IRental>? GetEquipmentRentalHistory(int equipmentId)
        {
            var rentals = _context.Rentals
                .Where(r => r.EquipmentId == equipmentId)
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToList();

            return rentals.Count != 0 ? rentals : null;
        }

        public IEnumerable<IRental>? GetOverdueRentals()
        {
            // Active rentals where DueDate < Now and not yet returned
            var rentals = _context.Rentals
                .Where(r => r.IsActive && r.DueDate < DateTime.UtcNow && r.ReturnedAt == null)
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .OrderBy(r => r.DueDate)
                .ToList();


            return rentals.Count != 0 ? rentals : null;
        }
    }
}