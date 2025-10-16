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

        public Task<IEnumerable<ICustomer>> GetAllCustomersAsync(int page = 1)
        {
            var allCustomers = _unitOfWork.Customers.ListAllCustomers();
            int skip = (page - 1) * PageSize;
            var paginatedCustomers = allCustomers.Skip(skip).Take(PageSize);
            return Task.FromResult(paginatedCustomers);
        }

        public Task<ICustomer> GetCustomerByIdAsync(int id)
        {
            var customer = _unitOfWork.Customers.GetCustomerDetails(id);
            return Task.FromResult(customer);
        }

        public Task AddCustomerAsync(ICustomer newCustomer)
        {
            _unitOfWork.Customers.CreateCustomer(newCustomer);
            _unitOfWork.SaveChanges();
            return Task.CompletedTask;
        }

        public Task UpdateCustomerAsync(int id, ICustomer updatedCustomer)
        {
            var existingCustomer = _unitOfWork.Customers.GetCustomerDetails(id) ?? throw new KeyNotFoundException($"Customer with ID {id} not found.");
            existingCustomer.Name = updatedCustomer.Name;
            existingCustomer.Email = updatedCustomer.Email;
            existingCustomer.UserName = updatedCustomer.UserName;
            existingCustomer.Password = updatedCustomer.Password;
            existingCustomer.Role = updatedCustomer.Role;

            _unitOfWork.Customers.UpdateCustomer(existingCustomer);
            _unitOfWork.SaveChanges();
            return Task.CompletedTask;
        }

        public Task DeleteCustomerAsync(int id)
        {
            var customer = _unitOfWork.Customers.GetCustomerDetails(id) ?? throw new KeyNotFoundException($"Customer with ID {id} not found.");
            _unitOfWork.Customers.DeleteCustomer(id);
            _unitOfWork.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<object>> GetCustomerRentalHistoryAsync(int customerId)
        {
            var rentals = _unitOfWork.Customers.GetCustomerRentalHistory(customerId);
            var rentalHistory = rentals.Select(r => new
            {
                RentalId = r.Id,
                EquipmentName = r.Equipment?.Name,
                IssuedAt = r.IssuedAt,
                DueDate = r.DueDate,
                ReturnedAt = r.ReturnedAt,
                Status = r.ReturnedAt.HasValue ? "Completed" : "Active"
            });
            return Task.FromResult<IEnumerable<object>>(rentalHistory);
        }

        public Task<object> GetCustomerActiveRentalAsync(int customerId)
        {
            var activeRental = _unitOfWork.Customers.GetCustomerActiveRental(customerId);
            if (activeRental == null)
            {
                return Task.FromResult<object>(null);
            }

            var response = new
            {
                RentalId = activeRental.Id,
                EquipmentName = activeRental.Equipment?.Name,
                EquipmentCategory = activeRental.Equipment?.Category,
                IssuedAt = activeRental.IssuedAt,
                DueDate = activeRental.DueDate,
                DaysRented = (DateTime.Now - activeRental.IssuedAt).Days
            };
            return Task.FromResult<object>(response);
        }
    }
}