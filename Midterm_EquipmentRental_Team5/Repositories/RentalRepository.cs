using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories
{
    public class RentalRepository: IRentalRepository
    {
        private readonly AppDbContext _context;

        public RentalRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CancelRental(int id)
        {
            throw new NotImplementedException();
        }

        public void ExtendRental(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Rental> GetActiveRentals()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Rental> GetAllRentals()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Rental> GetCompletedRentals()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Rental> GetEquipmentRentalHistory(int equipmentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Rental> GetOverdueRentals()
        {
            throw new NotImplementedException();
        }

        public Rental? GetRentalDetails(int id)
        {
            throw new NotImplementedException();
        }

        public Rental IssueEquipment(Rental rental)
        {
            throw new NotImplementedException();
        }

        public Rental ReturnEquipment(int id)
        {
            throw new NotImplementedException();
        }
    }
}
