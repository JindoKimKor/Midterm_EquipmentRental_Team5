using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface ICustomerServices
    {
        IEnumerable<ICustomer>? GetAllCustomersAsync(int page = 1);
        IEnumerable<ICustomer>? GetUnactiveCustomersAsync();
        ICustomer? GetCustomerByIdAsync(int id);
        void AddCustomerAsync(ICustomer newCustomer);
        ICustomer? UpdateCustomerAsync(int id, ICustomer updatedCustomer);
        ICustomer? DeleteCustomerAsync(int id);
        IEnumerable<IRental>? GetCustomerRentalHistoryAsync(int customerId);
        IRental? GetCustomerActiveRentalAsync(int customerId);
    }
}