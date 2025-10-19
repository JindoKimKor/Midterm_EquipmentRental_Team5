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

        public ICustomer CreateCustomer(ICustomer customer)
        {
            _context.Customers.Add((Customer)customer);
            return customer;
        }

        public ICustomer? DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            return customer;
        }

        public IRental? GetCustomerActiveRental(int id)
        {
            return _context.Rentals
                .Include(r => r.Equipment)
                .Include(r => r.Customer)
                .First(r => r.CustomerId == id && r.IsActive);
        }


        public ICustomer? GetCustomerDetails(int id)
        {
            return _context.Customers
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<IRental>? GetCustomerRentalHistory(int id)
        {
            return _context.Rentals
                .Include(r => r.Equipment)
                .Where(r => r.CustomerId == id);
        }

        public IEnumerable<ICustomer>? ListAllCustomers()
        {
            return _context.Customers.ToList() ?? null;
        }

        public ICustomer? UpdateCustomer(ICustomer customer)
        {
            var existingCustomer = _context.Customers.Find(customer.Id);
            if (existingCustomer != null)
            {
                _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
            }
            return existingCustomer;
        }
        public ICustomer? GetCustomerByPasswordAndUsername(ILoginRequest loginRequest)
        {
            return _context.Customers.FirstOrDefault(c => loginRequest.Password == c.Password && loginRequest.Username == c.UserName);
        }
    }
}