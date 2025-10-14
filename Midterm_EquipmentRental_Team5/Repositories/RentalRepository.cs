using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Midterm_EquipmentRental_Team5.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _context;

        public RentalRepository(AppDbContext context)
        {
            _context = context;
        }

        public Rental IssueEquipment(Rental rental)
        {
            // When issuing a rental, StartDate = now, EndDate = null, IsCancelled = false
            rental.StartDate = DateTime.UtcNow;
            rental.EndDate = null;
            rental.IsCancelled = false;
            _context.Rentals.Add(rental);
            _context.SaveChanges();
            return rental;
        }

        public Rental ReturnEquipment(int id)
        {
            var rental = _context.Rentals.Find(id);
            if (rental == null) return null;

            rental.EndDate = DateTime.UtcNow;
            _context.SaveChanges();
            return rental;
        }

        public void CancelRental(int id)
        {
            var rental = _context.Rentals.Find(id);
            if (rental == null) return;

            rental.IsCancelled = true;
            _context.SaveChanges();
        }

        public void ExtendRental(int id)
        {
            var rental = _context.Rentals.Find(id);
            if (rental == null) return;

            // Example: extend due date by 7 days (adjust as needed)
            rental.DueDate = rental.DueDate.AddDays(7);
            _context.SaveChanges();
        }

        public IEnumerable<Rental> GetActiveRentals()
        {
            // Active: Not cancelled, EndDate null, and DueDate not passed
            return _context.Rentals
                .Where(r => !r.IsCancelled && r.EndDate == null && r.DueDate >= DateTime.UtcNow)
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToList();
        }

        public IEnumerable<Rental> GetAllRentals()
        {
            return _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToList();
        }

        public IEnumerable<Rental> GetCompletedRentals()
        {
            // Completed: EndDate not null and not cancelled
            return _context.Rentals
                .Where(r => r.EndDate != null && !r.IsCancelled)
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToList();
        }

        public IEnumerable<Rental> GetEquipmentRentalHistory(int equipmentId)
        {
            return _context.Rentals
                .Where(r => r.EquipmentId == equipmentId)
                .Include(r => r.Customer)
                .ToList();
        }

        public IEnumerable<Rental> GetOverdueRentals()
        {
            // Overdue: Not cancelled, EndDate null, DueDate in the past
            return _context.Rentals
                .Where(r => !r.IsCancelled && r.EndDate == null && r.DueDate < DateTime.UtcNow)
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToList();
        }

        public Rental? GetRentalDetails(int id)
        {
            return _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .FirstOrDefault(r => r.Id == id);
        }
    }
}
