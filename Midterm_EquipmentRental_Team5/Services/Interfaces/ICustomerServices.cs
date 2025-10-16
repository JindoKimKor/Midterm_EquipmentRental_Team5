using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface ICustomerServices
    {
        Task<IEnumerable<ICustomer>> GetAllCustomersAsync(int page = 1);
        Task<ICustomer> GetCustomerByIdAsync(int id);
        Task AddCustomerAsync(ICustomer newCustomer);
        Task UpdateCustomerAsync(int id, ICustomer updatedCustomer);
        Task DeleteCustomerAsync(int id);
        Task<IEnumerable<object>> GetCustomerRentalHistoryAsync(int customerId);
        Task<object> GetCustomerActiveRentalAsync(int customerId);
    }
}