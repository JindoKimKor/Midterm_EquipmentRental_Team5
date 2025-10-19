using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface ICustomerServices
    {
        Task<IEnumerable<ICustomer>?> GetAllCustomersAsync(int page = 1);
        Task<ICustomer?> GetCustomerByIdAsync(int id);
        Task AddCustomerAsync(ICustomer newCustomer);
        Task<ICustomer?> UpdateCustomerAsync(int id, ICustomer updatedCustomer);
        Task<ICustomer?> DeleteCustomerAsync(int id);
        Task<IEnumerable<IRental>?> GetCustomerRentalHistoryAsync(int customerId);
        Task<IRental?> GetCustomerActiveRentalAsync(int customerId);
    }
}