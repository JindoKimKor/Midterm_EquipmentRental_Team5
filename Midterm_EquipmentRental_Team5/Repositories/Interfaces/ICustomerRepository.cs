using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        ICustomer CreateCustomer(ICustomer customer);
        ICustomer? DeleteCustomer(int id);
        ICustomer? UpdateCustomer(ICustomer customer);
        ICustomer? GetCustomerDetails(int id);
        ICustomer? GetCustomerByPasswordAndUsername(ILoginRequest loginRequest);
        IRental? GetCustomerActiveRental(int id);
        IEnumerable<IRental>? GetCustomerRentalHistory(int id);
        IEnumerable<ICustomer>? ListAllCustomers();
    }
}
