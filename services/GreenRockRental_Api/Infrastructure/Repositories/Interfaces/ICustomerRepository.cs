using GreenRockRental_Api.Domain.Entities;
using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Infrastructure.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        void CreateCustomer(ICustomer customer);
        Task DeleteCustomer(int id);
        Task<IRental?> GetCustomerActiveRental(int id);
        Task<IEnumerable<ICustomer>> GetCustomersUnactiveRental();
        Task<ICustomer?> GetCustomerDetails(int id);
        Task<IEnumerable<IRental>> GetCustomerRentalHistory(int id);
        Task<IEnumerable<ICustomer>> ListAllCustomers();
        Task UpdateCustomer(ICustomer customer);
        Task<ICustomer?> GetCustomerByPasswordAndUsername(ILoginRequest loginRequest);
        Task<ICustomer?> GetCustomerByEmail(string email);
    }
}
