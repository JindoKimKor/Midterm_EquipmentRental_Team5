using Midterm_EquipmentRental_Team5.Models;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface IRentalRepository
    {
        IEnumerable<Rental> GetAllRentals();
        Rental? GetRentalDetails(int id);
        Rental IssueEquipment(Rental rental);
        Rental ReturnEquipment(int id);
        IEnumerable<Rental> GetActiveRentals();
        IEnumerable<Rental> GetCompletedRentals();
        IEnumerable<Rental> GetOverdueRentals();
        IEnumerable<Rental> GetEquipmentRentalHistory(int equipmentId);
        void ExtendRental(int id);
        void CancelRental(int id);
    }
}
