using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        void CreateCustomer(ICustomer customer);
        void DeleteCustomer(int id);
        IRental? GetCustomerActiveRental(int id);
        IEnumerable<ICustomer> GetCustomersUnactiveRental();
        ICustomer? GetCustomerDetails(int id);
        IEnumerable<IRental> GetCustomerRentalHistory(int id);
        IEnumerable<ICustomer> ListAllCustomers();
        void UpdateCustomer(ICustomer customer);
        ICustomer? GetCustomerByPasswordAndUsername(ILoginRequest loginRequest);
    }
}
