using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
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