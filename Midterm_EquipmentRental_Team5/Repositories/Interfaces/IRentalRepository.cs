using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface IRentalRepository
    {

        IRental CreateRental(IRental rental); // Fixed: should take Rental and return Rental
        IEnumerable<IRental> GetAllRentals();

        IRental? GetRentalDetails(int id);

        IRental IssueEquipment(IRental rental, DateTime dueDate);

        IRental? ReturnEquipment(int id);

        void CancelRental(int id);

        void ExtendRental(int id);

        IEnumerable<IRental> GetActiveRentals();

        IEnumerable<IRental> GetCompletedRentals();

        IEnumerable<IRental> GetEquipmentRentalHistory(int equipmentId);

        IEnumerable<IRental> GetOverdueRentals();

        void UpdateRental(IRental rental);

    }
}
