using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Midterm_EquipmentRental_Team5.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _context;

        public RentalRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public Rental CreateRental(Rental rental)
        {
            _context.Rentals.Add(rental);
            return rental;
        }
        
        public void UpdateRental(Rental rental)
        {
            _context.Rentals.Update(rental);
        }

        public IEnumerable<Rental> GetAllRentals()
        {
            return _context.Rentals
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

        public Rental IssueEquipment(Rental rental, DateTime dueDate)
        {
            rental.IssuedAt = DateTime.UtcNow;
            rental.DueDate = dueDate;
            rental.IsActive = true;
            _context.Rentals.Add(rental);
            return rental;
        }

        public Rental? ReturnEquipment(int id)
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

        public IEnumerable<Rental> GetActiveRentals()
        {
            return _context.Rentals
                .Where(r => r.IsActive)
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .ToList();
        }

        public IEnumerable<Rental> GetCompletedRentals()
        {
            // Completed: EndDate not null and not cancelled
            return _context.Rentals
                .Where(r => !r.IsActive && r.ReturnedAt != null)
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
            // Active rentals where DueDate < Now and not yet returned
            return _context.Rentals
                .Where(r => r.IsActive && r.DueDate < DateTime.UtcNow && r.ReturnedAt == null)
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .OrderBy(r => r.DueDate) // Show oldest overdue first
                .ToList();
        }
    }
}
