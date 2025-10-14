using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class CustomerServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Customer>> GetAllCustomersAsync(int page = 1)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetCustomerByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task CreateCustomerAsync(CreateCustomerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCustomerAsync(int id, UpdateCustomerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCustomerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetCustomerRentalHistoryAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetCustomerActiveRentalAsync(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
