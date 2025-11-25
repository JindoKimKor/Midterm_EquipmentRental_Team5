using GreenRockRental_Api.Infrastructure.Persistence;
using GreenRockRental_Api.Domain.Entities;
using GreenRockRental_Api.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Infrastructure.Repositories
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
            var customerEntity = customer as Customer ?? throw new InvalidCastException("customer must be of type Customer");
            _context.Customers.Add(customerEntity);
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }

        public async Task<IRental?> GetCustomerActiveRental(int id)
        {
            var customers = await _context.Rentals
                .Include(r => r.Equipment)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.CustomerId == id && r.IsActive);

            return customers ?? null;
        }
        public async Task<IEnumerable<ICustomer>> GetCustomersUnactiveRental()
        {
            return await _context.Customers
                .Where(c => !_context.Rentals.Any(r => r.CustomerId == c.Id && r.IsActive))
                .ToListAsync();
        }


        public async Task<ICustomer?> GetCustomerDetails(int id)
        {
            var customers = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id);

            return customers ?? null;
        }

        public async Task<IEnumerable<IRental>> GetCustomerRentalHistory(int id)
        {
            return await _context.Rentals
                .Include(r => r.Equipment)
                .Where(r => r.CustomerId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<ICustomer>> ListAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task UpdateCustomer(ICustomer customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(customer.Id);
            if (existingCustomer != null)
            {
                _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
            }
        }

        public async Task<ICustomer?> GetCustomerByPasswordAndUsername(ILoginRequest loginRequest)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => loginRequest.Password == c.Password && loginRequest.Username == c.UserName);

            return customer ?? null;
        }

        public async Task<ICustomer?> GetCustomerByEmail(string email)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email != null && c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return customer ?? null;
        }
    }
}