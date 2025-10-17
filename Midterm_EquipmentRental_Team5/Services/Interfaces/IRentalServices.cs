using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface IRentalServices
    {
        Task<IEnumerable<IRental>> GetAllRentalsAsync(int page = 1);
        Task<IRental> GetRentalByIdAsync(int id);
        Task IssueEquipmentAsync(IIssueRequest request);
        Task ReturnEquipmentAsync(IReturnRequest request);
        Task<IEnumerable<IRental>> GetActiveRentalsAsync();
        Task<IEnumerable<IRental>> GetCompletedRentalsAsync();
        Task<IEnumerable<IRental>> GetOverdueRentalsAsync();
        Task<IEnumerable<IRental>> GetRentalHistoryByEquipmentAsync(int equipmentId);
        Task ExtendRentalAsync(int rentalId, IExtensionRequest request);
        Task CancelRentalAsync(int rentalId);
    }
}