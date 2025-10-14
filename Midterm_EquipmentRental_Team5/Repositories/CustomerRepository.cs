using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            _context.SaveChanges();
            return customer;
        }

        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }

        public Rental? GetCustomerActiveRental(int id)
        {
            return _context.Rentals
                .Include(r => r.EquipmentId)
                .FirstOrDefault(r => r.CustomerId == id && r.IsActive);
        }

        public Customer? GetCustomerDetails(int id)
        {
            // Include related rentals or other navigation properties as needed
            return _context.Customers
                .Include(c => c.) // Optional: load rental history
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Rental> GetCustomerRentalHistory(int id)
        {
            return _context.Rentals
                .Include(r => r.EquipmentId) // Optional: load related equipment details
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
                _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
                _context.SaveChanges();
            }
        }
    }
}
