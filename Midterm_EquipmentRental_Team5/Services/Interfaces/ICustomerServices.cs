using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Midterm_EquipmentRental_Team5.Models;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface ICustomerServices
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync(int page = 1);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task AddCustomerAsync(Customer newCustomer);
        Task UpdateCustomerAsync(int id, Customer updatedCustomer);
        Task DeleteCustomerAsync(int id);
        Task<IEnumerable<object>> GetCustomerRentalHistoryAsync(int customerId);
        Task<object> GetCustomerActiveRentalAsync(int customerId);
    }
}