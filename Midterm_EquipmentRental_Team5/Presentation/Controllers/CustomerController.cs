using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;
using Midterm_EquipmentRental_Team5.Application.Interfaces;
using Midterm_EquipmentRental_Team5.Application.DTOs;
using Microsoft.AspNetCore.SignalR;
using Midterm_EquipmentRental_Team5.Presentation.Hubs;

namespace Midterm_EquipmentRental_Team5.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController(ICustomerServices customerServices) : ControllerBase
    {
        private readonly ICustomerServices _customerService = customerServices;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<CustomerListDto>> GetAllCustomers(int page = 1)
        {
            try
            {
                var customers = _customerService.GetAllCustomers(page) ?? throw new KeyNotFoundException();
                var dtos = customers.Select(c => new CustomerListDto
                {
                    Id = c.Id,
                    UserName = c.UserName,
                    Email = c.Email,
                    Phone = c.Phone,
                    Role = c.Role,
                    Active = c.Active
                });
                return Ok(dtos);
            }
            catch (KeyNotFoundException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("unactive-customer")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<CustomerListDto>> GetUnactiveCustomers(int page = 1)
        {
            try
            {
                var customers = _customerService.GetUnactiveCustomers() ?? throw new KeyNotFoundException();
                var dtos = customers.Select(c => new CustomerListDto
                {
                    Id = c.Id,
                    UserName = c.UserName,
                    Email = c.Email,
                    Phone = c.Phone,
                    Role = c.Role,
                    Active = c.Active
                });
                return Ok(dtos);
            }
            catch (KeyNotFoundException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET /api/customers/{id} - Get customer details
        [HttpGet("{id}")]
        public ActionResult<CustomerDetailDto> GetCustomer(int id)
        {
            try
            {
                // Get current user's ID from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Return 403 if user tries to access another user's data
                if (userRole != "Admin" && currentUserId != id) return Forbid();

                var customer = _customerService.GetCustomerById(id) ?? throw new KeyNotFoundException();

                var dto = new CustomerDetailDto
                {
                    Id = customer.Id,
                    UserName = customer.UserName,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address,
                    City = customer.City,
                    ZipCode = customer.ZipCode,
                    State = customer.State,
                    Role = customer.Role,
                    Active = customer.Active,
                    CreatedAt = customer.CreatedAt
                };

                return Ok(dto);
            }
            catch (KeyNotFoundException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /api/customers - Create new customer (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            try
            {
                var customer = new Customer
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    Password = request.Password,
                    Phone = request.Phone,
                    Address = request.Address,
                    City = request.City,
                    ZipCode = request.ZipCode,
                    State = request.State,
                    Role = request.Role,
                    Active = true,
                    CreatedAt = DateTime.UtcNow
                };
                _customerService.AddCustomer(customer);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/customers/{id} - Update customer
        [HttpPut("{id}")]
        public ActionResult UpdateCustomer(int id, [FromBody] UpdateCustomerRequest request)
        {
            try
            {
                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only update their own data (except role), admins can update all
                if (userRole != "Admin" && currentUserId != id) return Forbid();

                var customer = _customerService.GetCustomerById(id) ?? throw new KeyNotFoundException();

                // If user is not admin, prevent role and active status change
                if (userRole != "Admin")
                {
                    request.Role = customer.Role; // Keep original role
                    request.Active = customer.Active; // Keep original active status
                }

                var updatedCustomer = new Customer
                {
                    Id = id,
                    UserName = request.UserName,
                    Email = request.Email,
                    Password = customer.Password, // Don't allow password change via this endpoint
                    Phone = request.Phone,
                    Address = request.Address,
                    City = request.City,
                    ZipCode = request.ZipCode,
                    State = request.State,
                    Role = request.Role,
                    Active = request.Active,
                    CreatedAt = customer.CreatedAt
                };

                _customerService.UpdateCustomer(id, updatedCustomer);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /api/customers/{id} - Delete customer and rental history (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteCustomer(int id)
        {
            try
            {
                _customerService.DeleteCustomer(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/customers/{id}/rentals - Get customer rental history
        [HttpGet("{id}/rentals")]
        public ActionResult<IEnumerable<RentalHistoryDto>> GetCustomerRentalHistory(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only view their own rental history
                if (userRole != "Admin" && currentUserId != id) return Forbid();

                var rentalHistory = _customerService.GetCustomerRentalHistory(id) ?? throw new KeyNotFoundException();
                var dtos = rentalHistory.Select(r => new RentalHistoryDto
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer?.UserName ?? "Unknown",
                    EquipmentId = r.EquipmentId,
                    EquipmentName = r.Equipment?.Name ?? "Unknown",
                    IssuedAt = r.IssuedAt,
                    DueDate = r.DueDate,
                    ReturnedAt = r.ReturnedAt,
                    OverdueFee = r.OverdueFee,
                    DaysRented = r.ReturnedAt.HasValue ? (int)(r.ReturnedAt.Value - r.IssuedAt).TotalDays : (int)(DateTime.UtcNow - r.IssuedAt).TotalDays
                });
                return Ok(dtos);
            }
            catch (KeyNotFoundException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/customers/{id}/active-rental - Get customer's active rental
        [HttpGet("{id}/active-rental")]
        public ActionResult<IEnumerable<RentalDetailDto>> GetCustomerActiveRentals(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only view their own active rental
                if (userRole != "Admin" && currentUserId != id) return Forbid();

                var activeRental = _customerService.GetCustomerActiveRental(id) ?? throw new KeyNotFoundException();
                var dtos = activeRental.Select(r => new RentalDetailDto
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer?.UserName ?? "Unknown",
                    CustomerEmail = r.Customer?.Email ?? "Unknown",
                    EquipmentId = r.EquipmentId,
                    EquipmentName = r.Equipment?.Name ?? "Unknown",
                    EquipmentCategory = r.Equipment?.Category ?? "Unknown",
                    RentalPrice = r.Equipment?.RentalPrice ?? 0,
                    IssuedAt = r.IssuedAt,
                    DueDate = r.DueDate,
                    ReturnedAt = r.ReturnedAt,
                    IsActive = r.IsActive,
                    OverdueFee = r.OverdueFee,
                    ExtensionReason = r.ExtensionReason
                });

                return Ok(dtos);
            }
            catch (KeyNotFoundException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}