using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Models.DTOs;
using Midterm_EquipmentRental_Team5.Models.Interfaces;
using Midterm_EquipmentRental_Team5.Services.Interfaces;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalController : ControllerBase
    {
        private readonly IRentalServices _rentalService;

        public RentalController(IRentalServices rentalService)
        {
            _rentalService = rentalService;
        }

        // GET /api/rentals - Get all rentals (Admin sees all, User sees own)
        [HttpGet]
        public ActionResult<IEnumerable<IRental>> GetAllRentals(int page = 1)
        {
            try
            {
                var rentals = _rentalService.GetAllRentalsAsync(page);

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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/rentals/{id} - Get rental details
        [HttpGet("{id}")]
        public ActionResult<IRental> GetRentalDetails(int id)
        {
            try
            {
                var rental = _rentalService.GetRentalByIdAsync(id);

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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /api/rentals/issue - Issue equipment (Admin can assign to any customer, User can only issue to themselves)
        [HttpPost("issue")]
        public ActionResult IssueEquipment([FromBody] IssueRequest issueRequest)
        {
            try
            {
                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // If user is not admin, they can only issue to themselves
                if (userRole != "Admin")
                {
                    if (issueRequest.CustomerId != currentUserId) { return Unauthorized(); }
                    issueRequest.CustomerId = currentUserId;
                }

                _rentalService.IssueEquipmentAsync(issueRequest);
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
        public ActionResult ReturnEquipment([FromBody] ReturnRequest returnRequest)
        {
            try
            {
                // Get rental to check ownership
                var rental = _rentalService.GetRentalByIdAsync(returnRequest.RentalId);

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

                _rentalService.ReturnEquipmentAsync(returnRequest);
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
        public ActionResult<IEnumerable<IRental>> GetActiveRentals()
        {
            try
            {
                var activeRentals = _rentalService.GetActiveRentalsAsync();

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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/rentals/completed - Get completed rentals
        [HttpGet("completed")]
        public ActionResult<IEnumerable<IRental>> GetCompletedRentals()
        {
            try
            {
                var completedRentals = _rentalService.GetCompletedRentalsAsync();

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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/rentals/overdue - Get overdue rentals (Admin only)
        [HttpGet("overdue")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<IRental>> GetOverdueRentals()
        {
            try
            {
                var overdueRentals = _rentalService.GetOverdueRentalsAsync();
                return Ok(overdueRentals);
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

        // GET /api/rentals/equipment/{equipmentId} - Get equipment rental history
        [HttpGet("equipment/{equipmentId}")]
        public ActionResult<IEnumerable<IRental>> GetEquipmentRentalHistory(int equipmentId)
        {
            try
            {
                var rentalHistory = _rentalService.GetRentalHistoryByEquipmentAsync(equipmentId);
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

        // PUT /api/rentals/{id} - Extend rental (Admin only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult ExtendRental(int id, [FromBody] ExtensionRequest extensionRequest)
        {
            try
            {
                _rentalService.ExtendRentalAsync(id, extensionRequest);
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
        public ActionResult CancelRental(int id)
        {
            try
            {
                _rentalService.CancelRentalAsync(id);
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