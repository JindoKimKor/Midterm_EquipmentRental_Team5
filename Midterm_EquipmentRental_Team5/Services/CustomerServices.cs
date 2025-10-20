using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;
using Midterm_EquipmentRental_Team5.Services.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int PageSize = 100;

        public CustomerServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ICustomer>? GetAllCustomers(int page = 1)
        {
            var customers = _unitOfWork.Customers.ListAllCustomers();
            if (customers.Any())
            {
                int skip = (page - 1) * PageSize;
                customers = customers.Skip(skip).Take(PageSize);
            }
            return customers;
        }

        public IEnumerable<ICustomer>? GetUnactiveCustomers()
        {
            var customers = _unitOfWork.Customers.GetCustomersUnactiveRental();
            return customers.Any() ? customers : null;
        }


        public ICustomer? GetCustomerById(int id)
        {
            var customer = _unitOfWork.Customers.GetCustomerDetails(id);
            return customer;
        }

        public void AddCustomer(ICustomer newCustomer)
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

        public void UpdateCustomer(int id, ICustomer updatedCustomer)
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
        }

        public void DeleteCustomer(int id)
        {
            _unitOfWork.Customers.DeleteCustomer(id);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<IRental>? GetCustomerRentalHistory(int customerId)
        {
            var rentals = _unitOfWork.Customers.GetCustomerRentalHistory(customerId);
            return rentals.Any() ? rentals : null;
        }

        public IRental? GetCustomerActiveRental(int customerId)
        {
            var activeRental = _unitOfWork.Customers.GetCustomerActiveRental(customerId);
            return activeRental;
        }
    }
}