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
        public ActionResult<IEnumerable<IRental>> GetAllCustomers(int page = 1)
        {
            try
            {
                var customers = _customerService.GetAllCustomers(page);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("unactive-customer")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<IRental>> GetUnactiveCustomers(int page = 1)
        {
            try
            {
                var customers = _customerService.GetUnactiveCustomers();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET /api/customers/{id} - Get customer details
        [HttpGet("{id}")]
        public ActionResult<IRental> GetCustomer(int id)
        {
            try
            {
                // Get current user's ID from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Return 403 if user tries to access another user's data
                if (userRole != "Admin" && currentUserId != id) return Forbid();

                var customer = _customerService.GetCustomerById(id) ?? throw new KeyNotFoundException();

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
        public ActionResult CreateCustomer([FromBody] Customer newCustomer)
        {
            try
            {
                _customerService.AddCustomer(newCustomer);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/customers/{id} - Update customer
        [HttpPut("{id}")]
        public ActionResult UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
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
                    var customer = _customerService.GetCustomerById(id) ?? throw new KeyNotFoundException();
                    updatedCustomer.Role = customer.Role; // Keep original role
                }

                var existingCustomer = _customerService.UpdateCustomer(id, updatedCustomer) ?? throw new KeyNotFoundException();

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
        public ActionResult<IRental> DeleteCustomer(int id)
        {
            try
            {
                var result = _customerService.DeleteCustomer(id) ?? throw new KeyNotFoundException();
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
        public ActionResult<IEnumerable<IRental>> GetCustomerRentalHistory(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only view their own rental history
                if (userRole != "Admin" && currentUserId != id) return Forbid();

                var rentalHistory = _customerService.GetCustomerRentalHistory(id) ?? throw new KeyNotFoundException();
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
        public ActionResult<IEnumerable<IRental>> GetCustomerActiveRentals(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only view their own active rental
                if (userRole != "Admin" && currentUserId != id) return Forbid();

                var activeRental = _customerService.GetCustomerActiveRental(id) ?? throw new KeyNotFoundException();

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