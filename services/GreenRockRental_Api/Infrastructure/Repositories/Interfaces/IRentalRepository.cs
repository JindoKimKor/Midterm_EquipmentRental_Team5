using GreenRockRental_Api.Domain.Entities;
using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Infrastructure.Repositories.Interfaces
{
    public interface IRentalRepository
    {
        void CreateRental(IRental rental);
        void UpdateRental(IRental rental);
        Task<IEnumerable<IRental>> GetAllRentals();
        Task<IRental?> GetRentalDetails(int id);
        void IssueEquipment(IRental rental, DateTime dueDate);
        Task ReturnEquipment(int id);
        Task CancelRental(int id);
        Task ExtendRental(int id);
        Task<IEnumerable<IRental>> GetActiveRentals();
        Task<IEnumerable<IRental>> GetCompletedRentals();
        Task<IEnumerable<IRental>> GetEquipmentRentalHistory(int equipmentId);
        Task<IEnumerable<IRental>> GetOverdueRentals();
    }
}
