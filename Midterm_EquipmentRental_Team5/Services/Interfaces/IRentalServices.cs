using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface IRentalServices
    {
        IEnumerable<IRental>? GetAllRentalsAsync(int page = 1);
        IRental? GetRentalByIdAsync(int id);

        void IssueEquipmentAsync(IIssueRequest request);
        void ReturnEquipmentAsync(IReturnRequest request);

        IEnumerable<IRental>? GetActiveRentalsAsync();
        IEnumerable<IRental>? GetCompletedRentalsAsync();
        IEnumerable<IRental>? GetOverdueRentalsAsync();
        IEnumerable<IRental>? GetRentalHistoryByEquipmentAsync(int equipmentId);

        void ExtendRentalAsync(int rentalId, IExtensionRequest request);
        void CancelRentalAsync(int rentalId);
    }
}