using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Application.Interfaces
{
    public interface ICustomerServices
    {
        IEnumerable<ICustomer>? GetAllCustomers(int page = 1);

        IEnumerable<ICustomer>? GetUnactiveCustomers();

        ICustomer? GetCustomerById(int id);

        void AddCustomer(ICustomer newCustomer);

        void UpdateCustomer(int id, ICustomer updatedCustomer);

        void DeleteCustomer(int id);

        IEnumerable<IRental>? GetCustomerRentalHistory(int customerId);

        IRental? GetCustomerActiveRental(int customerId);
    }
}