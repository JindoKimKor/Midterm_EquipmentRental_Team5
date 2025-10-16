using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.DTOs;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface IRentalServices
    {
        Task<IEnumerable<Rental>> GetAllRentalsAsync(int page = 1);
        Task<Rental> GetRentalByIdAsync(int id);
        Task IssueEquipmentAsync(IssueRequest request);
        Task ReturnEquipmentAsync(ReturnRequest request);
        Task<IEnumerable<Rental>> GetActiveRentalsAsync();
        Task<IEnumerable<Rental>> GetCompletedRentalsAsync();
        Task<IEnumerable<Rental>> GetOverdueRentalsAsync();
        Task<IEnumerable<Rental>> GetRentalHistoryByEquipmentAsync(int equipmentId);
        Task ExtendRentalAsync(int rentalId, ExtensionRequest request);
        Task CancelRentalAsync(int rentalId);
    }
}