using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;
using Midterm_EquipmentRental_Team5.Services.Interfaces;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerServices _customerService;

        public CustomerController(ICustomerServices customerServices)
        {
            _customerService = customerServices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<IRental>>> GetAllCustomers(int page = 1)
        {
            try
            {
                var customers = await _customerService.GetAllCustomersAsync(page);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/customers/{id} - Get customer details
        [HttpGet("{id}")]
        public async Task<ActionResult<IRental>> GetCustomer(int id)
        {
            try
            {
                // Get current user's ID from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Return 403 if user tries to access another user's data
                if (userRole != "Admin" && currentUserId != id) return Forbid();

                var customer = await _customerService.GetCustomerByIdAsync(id) ?? throw new KeyNotFoundException();

                return Ok(customer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /api/customers - Create new customer (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateCustomer([FromBody] Customer newCustomer)
        {
            try
            {
                await _customerService.AddCustomerAsync(newCustomer);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/customers/{id} - Update customer
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            try
            {
                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only update their own data (except role), admins can update all
                if (userRole != "Admin" && currentUserId != id) return Forbid();

                // If user is not admin, prevent role change
                if (userRole != "Admin")
                {
                    var customer = await _customerService.GetCustomerByIdAsync(id) ?? throw new KeyNotFoundException();
                    updatedCustomer.Role = customer.Role; // Keep original role
                }

                var existingCustomer = await _customerService.UpdateCustomerAsync(id, updatedCustomer) ?? throw new KeyNotFoundException();

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /api/customers/{id} - Delete customer and rental history (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IRental>> DeleteCustomer(int id)
        {
            try
            {
                var result = await _customerService.DeleteCustomerAsync(id) ?? throw new KeyNotFoundException();
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/customers/{id}/rentals - Get customer rental history
        [HttpGet("{id}/rentals")]
        public async Task<ActionResult<IEnumerable<IRental>>> GetCustomerRentalHistory(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only view their own rental history
                if (userRole != "Admin" && currentUserId != id) return Forbid();

                var rentalHistory = await _customerService.GetCustomerRentalHistoryAsync(id) ?? throw new KeyNotFoundException();
                return Ok(rentalHistory);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/customers/{id}/active-rental - Get customer's active rental
        [HttpGet("{id}/active-rental")]
        public async Task<ActionResult<IEnumerable<IRental>>> GetCustomerActiveRentals(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only view their own active rental
                if (userRole != "Admin" && currentUserId != id) return Forbid();

                var activeRental = await _customerService.GetCustomerActiveRentalAsync(id) ?? throw new KeyNotFoundException();

                return Ok(activeRental);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}