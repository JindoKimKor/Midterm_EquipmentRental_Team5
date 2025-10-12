using Midterm_EquipmentRental_Team5.Models;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> ListAllCustomers();
        Customer? GetCustomerDetails(int id);
        Customer CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
        IEnumerable<Rental> GetCustomerRentalHistory(int id);
        Rental? GetCustomerActiveRental(int id);
    }
}
