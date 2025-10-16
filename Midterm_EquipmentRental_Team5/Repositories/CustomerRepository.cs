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
            // Pulls records from rentals table and compares the customerId to the agrument id and chesk if active
            return _context.Rentals
                .Include(r => r.EquipmentId)
                .FirstOrDefault(r => r.CustomerId == id && r.IsActive);
        }

        public Customer? GetCustomerDetails(int id)
        {
            return _context.Customers
                .Include(c => c.Id)
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Rental> GetCustomerRentalHistory(int id)
        {
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
                //Uses existing customer to grab entry from db and sets current value to new
                _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
            }
        }
    }
}
