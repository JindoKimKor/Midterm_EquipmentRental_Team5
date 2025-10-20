using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface IRentalRepository
    {

        void CreateRental(IRental rental);
        void UpdateRental(IRental rental);
        IEnumerable<IRental> GetAllRentals();
        IRental? GetRentalDetails(int id);
        void IssueEquipment(IRental rental, DateTime dueDate);
        void ReturnEquipment(int id);
        void CancelRental(int id);
        void ExtendRental(int id);
        IEnumerable<IRental> GetActiveRentals();
        IEnumerable<IRental> GetCompletedRentals();
        IEnumerable<IRental> GetEquipmentRentalHistory(int equipmentId);
        IEnumerable<IRental> GetOverdueRentals();

    }
}
