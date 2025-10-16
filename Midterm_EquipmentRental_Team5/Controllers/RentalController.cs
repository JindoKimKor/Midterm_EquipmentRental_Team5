using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.DTOs;
using Midterm_EquipmentRental_Team5.Services;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // All endpoints require authentication
    public class RentalController : ControllerBase
    {
        private readonly RentalServices _rentalService;

        public RentalController(RentalServices rentalService)
        {
            _rentalService = rentalService;
        }

        // GET /api/rentals - Get all rentals (Admin sees all, User sees own)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllRentals(int page = 1)
        {
            try
            {
                var rentals = await _rentalService.GetAllRentalsAsync(page);

                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Filter rentals for non-admin users
                if (userRole != "Admin")
                {
                    rentals = rentals.Where(r => r.CustomerId == currentUserId);
                }

                return Ok(rentals);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/rentals/{id} - Get rental details
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRentalDetails(int id)
        {
            try
            {
                var rental = await _rentalService.GetRentalByIdAsync(id);

                if (rental == null)
                {
                    return NotFound($"Rental with ID {id} not found.");
                }

                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only view their own rentals
                if (userRole != "Admin" && rental.CustomerId != currentUserId)
                {
                    return Forbid();
                }

                return Ok(rental);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /api/rentals/issue - Issue equipment (Admin can assign to any customer, User can only issue to themselves)
        [HttpPost("issue")]
        public async Task<ActionResult> IssueEquipment([FromBody] IssueRequest issueRequest)
        {
            try
            {
                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // If user is not admin, they can only issue to themselves
                if (userRole != "Admin")
                {
                    issueRequest.CustomerId = currentUserId;
                }

                await _rentalService.IssueEquipmentAsync(issueRequest);
                return Ok(new { Message = "Equipment issued successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /api/rentals/return - Return equipment
        [HttpPost("return")]
        public async Task<ActionResult> ReturnEquipment([FromBody] ReturnRequest returnRequest)
        {
            try
            {
                // Get rental to check ownership
                var rental = await _rentalService.GetRentalByIdAsync(returnRequest.RentalId);

                if (rental == null)
                {
                    return NotFound($"Rental with ID {returnRequest.RentalId} not found.");
                }

                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only return their own equipment
                if (userRole != "Admin" && rental.CustomerId != currentUserId)
                {
                    return Forbid();
                }

                await _rentalService.ReturnEquipmentAsync(returnRequest);
                return Ok(new { Message = "Equipment returned successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/rentals/active - Get active rentals
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<object>>> GetActiveRentals()
        {
            try
            {
                var activeRentals = await _rentalService.GetActiveRentalsAsync();

                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Filter for non-admin users
                if (userRole != "Admin")
                {
                    activeRentals = activeRentals.Where(r => r.CustomerId == currentUserId);
                }

                return Ok(activeRentals);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/rentals/completed - Get completed rentals
        [HttpGet("completed")]
        public async Task<ActionResult<IEnumerable<object>>> GetCompletedRentals()
        {
            try
            {
                var completedRentals = await _rentalService.GetCompletedRentalsAsync();

                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Filter for non-admin users
                if (userRole != "Admin")
                {
                    completedRentals = completedRentals.Where(r => r.CustomerId == currentUserId);
                }

                return Ok(completedRentals);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/rentals/overdue - Get overdue rentals (Admin only)
        [HttpGet("overdue")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<object>>> GetOverdueRentals()
        {
            try
            {
                var overdueRentals = await _rentalService.GetOverdueRentalsAsync();
                return Ok(overdueRentals);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/rentals/equipment/{equipmentId} - Get equipment rental history
        [HttpGet("equipment/{equipmentId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetEquipmentRentalHistory(int equipmentId)
        {
            try
            {
                var rentalHistory = await _rentalService.GetRentalHistoryByEquipmentAsync(equipmentId);
                return Ok(rentalHistory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/rentals/{id} - Extend rental (Admin only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ExtendRental(int id, [FromBody] ExtensionRequest extensionRequest)
        {
            try
            {
                await _rentalService.ExtendRentalAsync(id, extensionRequest);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /api/rentals/{id} - Cancel/Force return rental (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CancelRental(int id)
        {
            try
            {
                await _rentalService.CancelRentalAsync(id);
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
    }
}