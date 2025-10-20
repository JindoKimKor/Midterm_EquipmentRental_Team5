using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface IRentalServices
    {
        IEnumerable<IRental>? GetAllRentals(int page = 1);
        IRental? GetRentalById(int id);

        void IssueEquipment(IIssueRequest request);
        void ReturnEquipment(IReturnRequest request);

        IEnumerable<IRental>? GetActiveRentals();
        IEnumerable<IRental>? GetCompletedRentals();
        IEnumerable<IRental>? GetOverdueRentals();

        IEnumerable<IRental>? GetRentalHistoryByEquipment(int equipmentId);

        void ExtendRental(int rentalId, IExtensionRequest request);
        void CancelRental(int rentalId);
    }
}