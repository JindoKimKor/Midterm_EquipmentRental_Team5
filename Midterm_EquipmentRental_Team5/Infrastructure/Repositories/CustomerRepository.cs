using Midterm_EquipmentRental_Team5.Infrastructure.Persistence;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCustomer(ICustomer customer)
        {
            _context.Customers.Add((Customer)customer);
        }

        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }

        public IRental? GetCustomerActiveRental(int id)
        {
            var customers = _context.Rentals
                .Include(r => r.Equipment)
                .Include(r => r.Customer)
                .FirstOrDefault(r => r.CustomerId == id && r.IsActive);

            return customers ?? null;
        }
        public IEnumerable<ICustomer> GetCustomersUnactiveRental()
        {
            return _context.Customers
                .Where(c => !_context.Rentals.Any(r => r.CustomerId == c.Id && r.IsActive))
                .ToList();
        }


        public ICustomer? GetCustomerDetails(int id)
        {
            var customers = _context.Customers
                .FirstOrDefault(c => c.Id == id);

            return customers ?? null;
        }

        public IEnumerable<IRental> GetCustomerRentalHistory(int id)
        {
            return _context.Rentals
                .Include(r => r.Equipment)
                .Where(r => r.CustomerId == id)
                .ToList();
        }

        public IEnumerable<ICustomer> ListAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public void UpdateCustomer(ICustomer customer)
        {
            var existingCustomer = _context.Customers.Find(customer.Id);
            if (existingCustomer != null)
            {
                _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
            }
        }

        public ICustomer? GetCustomerByPasswordAndUsername(ILoginRequest loginRequest)
        {
            var customer = _context.Customers.FirstOrDefault(c => loginRequest.Password == c.Password && loginRequest.Username == c.UserName);

            return customer ?? null;
        }

        public ICustomer? GetCustomerByEmailAsync(string email)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Email != null && c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return customer ?? null;
        }
    }
}