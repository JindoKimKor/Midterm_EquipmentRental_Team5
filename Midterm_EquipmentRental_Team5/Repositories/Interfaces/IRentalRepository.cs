using Midterm_EquipmentRental_Team5.Models;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface IRentalRepository
    {
        IEnumerable<Rental> GetAllRentals();

        Rental? GetRentalDetails(int id);

        Rental IssueEquipment(Rental rental, DateTime dueDate);

        Rental? ReturnEquipment(int id);

        void CancelRental(int id);

        void ExtendRental(int id);

        IEnumerable<Rental> GetActiveRentals();

        IEnumerable<Rental> GetCompletedRentals();

        IEnumerable<Rental> GetEquipmentRentalHistory(int equipmentId);

        IEnumerable<Rental> GetOverdueRentals();
    }
}
