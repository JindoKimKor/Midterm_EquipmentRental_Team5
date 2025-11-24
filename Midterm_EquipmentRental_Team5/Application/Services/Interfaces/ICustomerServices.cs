using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Application.Interfaces
{
    public interface ICustomerServices
    {
        Task<IEnumerable<ICustomer>?> GetAllCustomers(int page = 1);

        Task<IEnumerable<ICustomer>?> GetUnactiveCustomers();

        Task<ICustomer?> GetCustomerById(int id);

        Task AddCustomer(ICustomer newCustomer);

        Task UpdateCustomer(int id, ICustomer updatedCustomer);

        Task DeleteCustomer(int id);

        Task<IEnumerable<IRental>?> GetCustomerRentalHistory(int customerId);

        Task<IRental?> GetCustomerActiveRental(int customerId);
    }
}