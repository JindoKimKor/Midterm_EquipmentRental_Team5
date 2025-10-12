using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;

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
            throw new NotImplementedException();
        }

        public void DeleteCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public Rental? GetCustomerActiveRental(int id)
        {
            throw new NotImplementedException();
        }

        public Customer? GetCustomerDetails(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Rental> GetCustomerRentalHistory(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> ListAllCustomers()
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
