using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<ICustomer> ListAllCustomers();
        ICustomer? GetCustomerDetails(int id);
        ICustomer CreateCustomer(ICustomer customer);
        void UpdateCustomer(ICustomer customer);
        void DeleteCustomer(int id);
        IEnumerable<IRental> GetCustomerRentalHistory(int id);
        IRental? GetCustomerActiveRental(int id);

        ICustomer GetCustomerByPasswordAndUsername(ILoginRequest loginRequest);
    }
}
