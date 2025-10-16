using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Services.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int PageSize = 10; // 10 items per page

        public CustomerServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Customer>> GetAllCustomersAsync(int page = 1)
        {
            // Get all customers from repository
            var allCustomers = _unitOfWork.Customers.ListAllCustomers();

            // Calculate skip and take for pagination
            int skip = (page - 1) * PageSize;
            var paginatedCustomers = allCustomers.Skip(skip).Take(PageSize);

            return Task.FromResult(paginatedCustomers);
        }

        public Task<Customer> GetCustomerByIdAsync(int id)
        {
            // Get customer by id from repository
            var customer = _unitOfWork.Customers.GetCustomerDetails(id);

            return Task.FromResult(customer);
        }

        public Task AddCustomerAsync(Customer newCustomer)
        {
            // Add customer to repository
            _unitOfWork.Customers.CreateCustomer(newCustomer);
            
            // Save changes to database
            _unitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task UpdateCustomerAsync(int id, Customer updatedCustomer)
        {
            // Get existing customer from repository
            var existingCustomer = _unitOfWork.Customers.GetCustomerDetails(id);

            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            // Update fields
            existingCustomer.Name = updatedCustomer.Name;
            existingCustomer.Email = updatedCustomer.Email;
            existingCustomer.UserName = updatedCustomer.UserName;
            existingCustomer.Password = updatedCustomer.Password;
            existingCustomer.Role = updatedCustomer.Role;

            // Update in repository
            _unitOfWork.Customers.UpdateCustomer(existingCustomer);
            
            // Save changes to database
            _unitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task DeleteCustomerAsync(int id)
        {
            // Get customer to delete
            var customer = _unitOfWork.Customers.GetCustomerDetails(id);

            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            // Delete all rentals associated with this customer first
            var customerRentals = _unitOfWork.Rentals.GetAllRentals()
                .Where(r => r.CustomerId == id);

            foreach (var rental in customerRentals)
            {
                _unitOfWork.Rentals.CancelRental(rental.Id);
            }

            // Delete customer from repository
            _unitOfWork.Customers.DeleteCustomer(id);
            
            // Save changes to database
            _unitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<object>> GetCustomerRentalHistoryAsync(int customerId)
        {
            // Get all rentals for the customer
            var rentals = _unitOfWork.Rentals.GetAllRentals()
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.IssuedAt);

            // Create response with equipment and rental details
            var rentalHistory = rentals.Select(r =>
            {
                var equipment = _unitOfWork.Equipment.GetSpecificEquipment(r.EquipmentId);
                return new
                {
                    RentalId = r.Id,
                    EquipmentName = equipment?.Name,
                    IssuedAt = r.IssuedAt,
                    DueDate = r.DueDate,
                    ReturnedAt = r.ReturnedAt,
                    Status = r.ReturnedAt.HasValue ? "Completed" : "Active"
                };
            });

            return Task.FromResult<IEnumerable<object>>(rentalHistory);
        }

        public Task<object> GetCustomerActiveRentalAsync(int customerId)
        {
            // Get active rental for the customer (where ReturnedAt is null)
            var activeRental = _unitOfWork.Rentals.GetAllRentals()
                .FirstOrDefault(r => r.CustomerId == customerId && r.ReturnedAt == null);

            if (activeRental == null)
            {
                return Task.FromResult<object>(null);
            }

            // Get equipment details
            var equipment = _unitOfWork.Equipment.GetSpecificEquipment(activeRental.EquipmentId);

            // Create response object
            var response = new
            {
                RentalId = activeRental.Id,
                EquipmentName = equipment?.Name,
                EquipmentCategory = equipment?.Category,
                IssuedAt = activeRental.IssuedAt,
                DueDate = activeRental.DueDate,
                DaysRented = (DateTime.Now - activeRental.IssuedAt).Days
            };

            return Task.FromResult<object>(response);
        }
    }
}