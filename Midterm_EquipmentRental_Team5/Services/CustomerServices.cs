using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;
using Midterm_EquipmentRental_Team5.Services.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int PageSize = 10;

        public CustomerServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ICustomer>? GetAllCustomersAsync(int page = 1)
        {
            var customers = _unitOfWork.Customers.ListAllCustomers();
            if (customers != null)
            {
                int skip = (page - 1) * PageSize;
                customers = customers.Skip(skip).Take(PageSize);
            }
            return customers;
        }

        public IEnumerable<ICustomer>? GetUnactiveCustomersAsync()
        {
            var customers = _unitOfWork.Customers.GetCustomersUnactiveRental();

            return customers;
        }


        public ICustomer? GetCustomerByIdAsync(int id)
        {
            var customer = _unitOfWork.Customers.GetCustomerDetails(id);
            return customer;
        }

        public void AddCustomerAsync(ICustomer newCustomer)
        {


            _unitOfWork.Customers.CreateCustomer(new Customer
            {
                Name = newCustomer.Name,
                Email = newCustomer.Email,
                UserName = newCustomer.UserName,
                Password = newCustomer.Password,
                Role = newCustomer.Role,
            });
            _unitOfWork.SaveChanges();
        }

        public ICustomer? UpdateCustomerAsync(int id, ICustomer updatedCustomer)
        {
            var existingCustomer = _unitOfWork.Customers.GetCustomerDetails(id);
            if (existingCustomer != null)
            {
                existingCustomer.Name = updatedCustomer.Name;
                existingCustomer.Email = updatedCustomer.Email;
                existingCustomer.UserName = updatedCustomer.UserName;
                existingCustomer.Password = updatedCustomer.Password;
                existingCustomer.Role = updatedCustomer.Role;

                _unitOfWork.Customers.UpdateCustomer(existingCustomer);
                _unitOfWork.SaveChanges();
            }
            return existingCustomer;
        }

        public ICustomer? DeleteCustomerAsync(int id)
        {
            var customer = _unitOfWork.Customers.GetCustomerDetails(id);
            _unitOfWork.Customers.DeleteCustomer(id);
            _unitOfWork.SaveChanges();
            return customer;
        }

        public IEnumerable<IRental>? GetCustomerRentalHistoryAsync(int customerId)
        {
            var rentals = _unitOfWork.Customers.GetCustomerRentalHistory(customerId) ?? null;
            return rentals;
        }

        public IRental? GetCustomerActiveRentalAsync(int customerId)
        {
            var activeRental = _unitOfWork.Customers.GetCustomerActiveRental(customerId) ?? null;
            return activeRental;
        }
    }
}