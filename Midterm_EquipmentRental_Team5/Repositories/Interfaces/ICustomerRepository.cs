using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<ICustomer>? ListAllCustomers();
        ICustomer? GetCustomerDetails(int id);
        ICustomer CreateCustomer(ICustomer customer);
        ICustomer? UpdateCustomer(ICustomer customer);
        ICustomer? DeleteCustomer(int id);
        IEnumerable<IRental>? GetCustomerRentalHistory(int id);
        IEnumerable<IRental>? GetCustomerActiveRentals(int id);

        ICustomer? GetCustomerByPasswordAndUsername(ILoginRequest loginRequest);
    }
}
