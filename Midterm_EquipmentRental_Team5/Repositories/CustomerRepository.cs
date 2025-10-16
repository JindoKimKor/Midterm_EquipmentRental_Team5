using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public Customer CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            return customer;
        }

        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }

        public Rental? GetCustomerActiveRental(int id)
        {
            // ✅ FIXED: Include Equipment navigation property, not EquipmentId scalar
            return _context.Rentals
                .Include(r => r.Equipment)  // Navigation property
                .Include(r => r.Customer)   // Navigation property
                .FirstOrDefault(r => r.CustomerId == id && r.IsActive);
        }

        public Customer? GetCustomerDetails(int id)
        {
            // ✅ FIXED: Removed Include(c => c.Id) - Id is a scalar property
            return _context.Customers
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Rental> GetCustomerRentalHistory(int id)
        {
            // ✅ Already correct - Equipment is a navigation property
            return _context.Rentals
                .Include(r => r.Equipment)
                .Where(r => r.CustomerId == id)
                .ToList();
        }

        public IEnumerable<Customer> ListAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public void UpdateCustomer(Customer customer)
        {
            var existingCustomer = _context.Customers.Find(customer.Id);
            if (existingCustomer != null)
            {
                // Uses existing customer to grab entry from db and sets current value to new
                _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
            }
        }
        public Customer GetCustomerByPasswordAndUsername(ILoginRequest loginRequest)
        {
            return _context.Customers.FirstOrDefault(c => loginRequest.Password == c.Password && loginRequest.Username == c.UserName);
        }
    }
}