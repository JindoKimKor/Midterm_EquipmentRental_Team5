using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Services.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Customer? ValidateLogin(string username, string password)
        {
            // Get all customers from repository
            var customers = _unitOfWork.Customers.ListAllCustomers();

            // Find customer with matching username and password (plain text comparison)
            var customer = customers.FirstOrDefault(c => 
                c.UserName == username && c.Password == password);

            return customer; // Returns null if not found
        }
    }
}