using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Application.Interfaces
{
    public interface IRentalServices
    {
        Task<IEnumerable<IRental>?> GetAllRentals(int page = 1);
        Task<IRental?> GetRentalById(int id);

        Task IssueEquipment(IIssueRequest request);
        Task ReturnEquipment(IReturnRequest request);

        Task<IEnumerable<IRental>?> GetActiveRentals();
        Task<IEnumerable<IRental>?> GetCompletedRentals();
        Task<IEnumerable<IRental>?> GetOverdueRentals();

        Task<IEnumerable<IRental>?> GetRentalHistoryByEquipment(int equipmentId);

        Task ExtendRental(int rentalId, IExtensionRequest request);
        Task CancelRental(int rentalId);
    }
}