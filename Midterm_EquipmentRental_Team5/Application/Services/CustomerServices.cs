using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;
using Midterm_EquipmentRental_Team5.Application.Interfaces;
using Midterm_EquipmentRental_Team5.Infrastructure.UnitOfWork;

namespace Midterm_EquipmentRental_Team5.Application.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int PageSize = 100;

        public CustomerServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ICustomer>?> GetAllCustomers(int page = 1)
        {
            var customers = await _unitOfWork.Customers.ListAllCustomers();
            if (customers.Any())
            {
                int skip = (page - 1) * PageSize;
                customers = customers.Skip(skip).Take(PageSize);
            }
            return customers;
        }

        public async Task<IEnumerable<ICustomer>?> GetUnactiveCustomers()
        {
            var customers = await _unitOfWork.Customers.GetCustomersUnactiveRental();
            return customers.Any() ? customers : null;
        }


        public async Task<ICustomer?> GetCustomerById(int id)
        {
            var customer = await _unitOfWork.Customers.GetCustomerDetails(id);
            return customer;
        }

        public async Task AddCustomer(ICustomer newCustomer)
        {
            _unitOfWork.Customers.CreateCustomer(new Customer
            {
                Name = newCustomer.Name,
                Email = newCustomer.Email,
                UserName = newCustomer.UserName,
                Password = newCustomer.Password,
                Role = newCustomer.Role,
            });
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCustomer(int id, ICustomer updatedCustomer)
        {
            var existingCustomer = await _unitOfWork.Customers.GetCustomerDetails(id);
            if (existingCustomer != null)
            {
                existingCustomer.Name = updatedCustomer.Name;
                existingCustomer.Email = updatedCustomer.Email;
                existingCustomer.UserName = updatedCustomer.UserName;
                existingCustomer.Password = updatedCustomer.Password;
                existingCustomer.Role = updatedCustomer.Role;

                await _unitOfWork.Customers.UpdateCustomer(existingCustomer);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteCustomer(int id)
        {
            await _unitOfWork.Customers.DeleteCustomer(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<IRental>?> GetCustomerRentalHistory(int customerId)
        {
            var rentals = await _unitOfWork.Customers.GetCustomerRentalHistory(customerId);
            return rentals.Any() ? rentals : null;
        }

        public async Task<IRental?> GetCustomerActiveRental(int customerId)
        {
            var activeRental = await _unitOfWork.Customers.GetCustomerActiveRental(customerId);
            return activeRental;
        }
    }
}