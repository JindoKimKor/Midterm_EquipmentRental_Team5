using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Application.DTOs;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;
using Midterm_EquipmentRental_Team5.Application.Interfaces;

namespace Midterm_EquipmentRental_Team5.Presentation.Controllers
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
        public async Task<ActionResult<IEnumerable<RentalListDto>>> GetAllRentals(int page = 1)
        {
            try
            {
                var rentals = await _rentalService.GetAllRentals(page) ?? throw new KeyNotFoundException();

                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Filter rentals for non-admin users
                if (userRole != "Admin")
                {
                    rentals = rentals.Where(r => r.CustomerId == currentUserId);
                }

                var dtos = rentals.Select(r => new RentalListDto
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer?.UserName ?? "Unknown",
                    EquipmentId = r.EquipmentId,
                    EquipmentName = r.Equipment?.Name ?? "Unknown",
                    IssuedAt = r.IssuedAt,
                    DueDate = r.DueDate,
                    ReturnedAt = r.ReturnedAt,
                    IsActive = r.IsActive,
                    OverdueFee = r.OverdueFee
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

        // GET /api/rentals/{id} - Get rental details
        [HttpGet("{id}")]
        public async Task<ActionResult<RentalDetailDto>> GetRentalDetails(int id)
        {
            try
            {
                var rental = await _rentalService.GetRentalById(id) ?? throw new KeyNotFoundException();

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

                var dto = new RentalDetailDto
                {
                    Id = rental.Id,
                    CustomerId = rental.CustomerId,
                    CustomerName = rental.Customer?.UserName ?? "Unknown",
                    CustomerEmail = rental.Customer?.Email ?? "Unknown",
                    EquipmentId = rental.EquipmentId,
                    EquipmentName = rental.Equipment?.Name ?? "Unknown",
                    EquipmentCategory = rental.Equipment?.Category ?? "Unknown",
                    RentalPrice = rental.Equipment?.RentalPrice ?? 0,
                    IssuedAt = rental.IssuedAt,
                    DueDate = rental.DueDate,
                    ReturnedAt = rental.ReturnedAt,
                    IsActive = rental.IsActive,
                    OverdueFee = rental.OverdueFee,
                    ExtensionReason = rental.ExtensionReason
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
                    if (issueRequest.CustomerId != currentUserId) { return Unauthorized(); }
                    issueRequest.CustomerId = currentUserId;
                }

                await _rentalService.IssueEquipment(issueRequest);
                return Ok();
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
                var rental = await _rentalService.GetRentalById(returnRequest.RentalId) ?? throw new KeyNotFoundException();

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

                await _rentalService.ReturnEquipment(returnRequest);
                return Ok();
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
        public async Task<ActionResult<IEnumerable<RentalListDto>>> GetActiveRentals()
        {
            try
            {
                var activeRentals = await _rentalService.GetActiveRentals() ?? throw new KeyNotFoundException();

                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Filter for non-admin users
                if (userRole != "Admin")
                {
                    activeRentals = activeRentals.Where(r => r.CustomerId == currentUserId);
                }

                var dtos = activeRentals.Select(r => new RentalListDto
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer?.UserName ?? "Unknown",
                    EquipmentId = r.EquipmentId,
                    EquipmentName = r.Equipment?.Name ?? "Unknown",
                    IssuedAt = r.IssuedAt,
                    DueDate = r.DueDate,
                    ReturnedAt = r.ReturnedAt,
                    IsActive = r.IsActive,
                    OverdueFee = r.OverdueFee
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

        // GET /api/rentals/completed - Get completed rentals
        [HttpGet("completed")]
        public async Task<ActionResult<IEnumerable<RentalHistoryDto>>> GetCompletedRentals()
        {
            try
            {
                var completedRentals = await _rentalService.GetCompletedRentals() ?? throw new KeyNotFoundException();

                // Get current user's ID and role from JWT token
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Filter for non-admin users
                if (userRole != "Admin")
                {
                    completedRentals = completedRentals.Where(r => r.CustomerId == currentUserId);
                }

                var dtos = completedRentals.Select(r => new RentalHistoryDto
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

        // GET /api/rentals/overdue - Get overdue rentals (Admin only)
        [HttpGet("overdue")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<RentalListDto>>> GetOverdueRentals()
        {
            try
            {
                var overdueRentals = await _rentalService.GetOverdueRentals() ?? throw new KeyNotFoundException();
                var dtos = overdueRentals.Select(r => new RentalListDto
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer?.UserName ?? "Unknown",
                    EquipmentId = r.EquipmentId,
                    EquipmentName = r.Equipment?.Name ?? "Unknown",
                    IssuedAt = r.IssuedAt,
                    DueDate = r.DueDate,
                    ReturnedAt = r.ReturnedAt,
                    IsActive = r.IsActive,
                    OverdueFee = r.OverdueFee
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

        // GET /api/rentals/equipment/{equipmentId} - Get equipment rental history
        [HttpGet("equipment/{equipmentId}")]
        public async Task<ActionResult<IEnumerable<RentalHistoryDto>>> GetEquipmentRentalHistory(int equipmentId)
        {
            try
            {
                var rentalHistory = await _rentalService.GetRentalHistoryByEquipment(equipmentId) ?? throw new KeyNotFoundException();
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

        // PUT /api/rentals/{id} - Extend rental (Admin only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ExtendRental(int id, [FromBody] ExtensionRequest extensionRequest)
        {
            try
            {
                await _rentalService.ExtendRental(id, extensionRequest);
                return Ok();
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
                await _rentalService.CancelRental(id);
                return Ok();
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