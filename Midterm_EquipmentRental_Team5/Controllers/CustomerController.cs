using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Services;
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
        public async Task<ActionResult<IEnumerable<object>>> GetAllCustomers(int page = 1)
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
        public async Task<ActionResult<object>> GetCustomer(int id)
        {
            try
            {
                // Get current user's ID from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole != "Admin" && currentUserId != id)
                {
                    return Forbid(); // Return 403 if user tries to access another user's data
                }

                var customer = await _customerService.GetCustomerByIdAsync(id);

                if (customer == null)
                {
                    return NotFound($"Customer with ID {id} not found.");
                }

                return Ok(customer);
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
                return CreatedAtAction(nameof(GetCustomer), new { id = newCustomer.Id }, newCustomer);
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
                if (userRole != "Admin" && currentUserId != id)
                {
                    return Forbid();
                }

                // If user is not admin, prevent role change
                if (userRole != "Admin")
                {
                    var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
                    updatedCustomer.Role = existingCustomer.Role; // Keep original role
                }

                await _customerService.UpdateCustomerAsync(id, updatedCustomer);
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
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
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

        // GET /api/customers/{id}/rentals - Get customer rental history
        [HttpGet("{id}/rentals")]
        public async Task<ActionResult<IEnumerable<object>>> GetCustomerRentalHistory(int id)
        {
            try
            {
                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only view their own rental history
                if (userRole != "Admin" && currentUserId != id)
                {
                    return Forbid();
                }

                var rentalHistory = await _customerService.GetCustomerRentalHistoryAsync(id);
                return Ok(rentalHistory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/customers/{id}/active-rental - Get customer's active rental
        [HttpGet("{id}/active-rental")]
        public async Task<ActionResult<object>> GetCustomerActiveRental(int id)
        {
            try
            {
                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only view their own active rental
                if (userRole != "Admin" && currentUserId != id)
                {
                    return Forbid();
                }

                var activeRental = await _customerService.GetCustomerActiveRentalAsync(id);

                if (activeRental == null)
                {
                    return Ok(new { Message = "No active rental found." });
                }

                return Ok(activeRental);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}