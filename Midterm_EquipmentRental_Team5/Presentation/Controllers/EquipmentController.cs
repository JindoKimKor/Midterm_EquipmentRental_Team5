using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;
using Midterm_EquipmentRental_Team5.Application.Interfaces;
using Midterm_EquipmentRental_Team5.Application.DTOs;

namespace Midterm_EquipmentRental_Team5.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // All endpoints require authentication
    public class EquipmentController(IEquipmentServices equipmentService, IRentalServices rentalServices) : ControllerBase
    {
        private readonly IEquipmentServices _equipmentService = equipmentService;
        private readonly IRentalServices _rentalService = rentalServices;

        // GET /api/equipment - Get all equipment with pagination
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentListDto>>> GetAllEquipment(int page = 1)
        {
            try
            {
                var equipment = await _equipmentService.GetAllEquipment(page) ?? throw new KeyNotFoundException();
                var dtos = equipment.Select(e => new EquipmentListDto
                {
                    Id = e.Id.Value,
                    Name = e.Name,
                    Category = e.Category,
                    Condition = e.Condition,
                    RentalPrice = e.RentalPrice,
                    IsAvailable = e.IsAvailable,
                    ImageUrl = e.ImageUrl
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

        // Get rental history for specific equipment
        [HttpGet("{id}/rental-history")]
        public async Task<ActionResult<IEnumerable<RentalHistoryDto>>> GetEquipmentRentalHistory(int id)
        {
            try
            {
                // First check if equipment exists
                var equipment = await _equipmentService.GetEquipmentById(id);
                if (equipment == null)
                {
                    return NotFound($"Equipment with ID {id} not found.");
                }

                // Get rental history for this equipment
                var rentalHistory = await _rentalService.GetRentalHistoryByEquipment(id);
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // GET /api/equipment/{id} - Get specific equipment details
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentDetailDto>> GetEquipment(int id)
        {
            try
            {
                var equipment = await _equipmentService.GetEquipmentById(id) ?? throw new KeyNotFoundException();
                var dto = new EquipmentDetailDto
                {
                    Id = equipment.Id.Value,
                    Name = equipment.Name,
                    Description = equipment.Description,
                    Category = equipment.Category,
                    Condition = equipment.Condition,
                    RentalPrice = equipment.RentalPrice,
                    IsAvailable = equipment.IsAvailable,
                    ImageUrl = equipment.ImageUrl,
                    CreatedAt = equipment.CreatedAt
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

        // POST /api/equipment - Add new equipment (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddEquipment([FromBody] CreateEquipmentRequest request)
        {
            try
            {
                var equipment = new Equipment
                {
                    Name = request.Name,
                    Description = request.Description,
                    Category = request.Category,
                    Condition = request.Condition,
                    RentalPrice = request.RentalPrice,
                    ImageUrl = request.ImageUrl,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow
                };
                await _equipmentService.AddEquipment(equipment);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/equipment/{id} - Update equipment (Admin only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateEquipment(int id, [FromBody] UpdateEquipmentRequest request)
        {
            try
            {
                var equipment = new Equipment
                {
                    Id = id,
                    Name = request.Name,
                    Description = request.Description,
                    Category = request.Category,
                    Condition = request.Condition,
                    RentalPrice = request.RentalPrice,
                    ImageUrl = request.ImageUrl,
                    IsAvailable = request.IsAvailable
                };
                await _equipmentService.UpdateEquipment(id, equipment);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /api/equipment/{id} - Delete equipment (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteEquipment(int id)
        {
            try
            {
                await _equipmentService.DeleteEquipment(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/equipment/available - Get list of available equipment
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<EquipmentListDto>>> GetAvailableEquipment()
        {
            try
            {
                var availableEquipment = await _equipmentService.GetAvailableEquipment() ?? throw new KeyNotFoundException();
                var dtos = availableEquipment.Select(e => new EquipmentListDto
                {
                    Id = e.Id.Value,
                    Name = e.Name,
                    Category = e.Category,
                    Condition = e.Condition,
                    RentalPrice = e.RentalPrice,
                    IsAvailable = e.IsAvailable,
                    ImageUrl = e.ImageUrl
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
        // GET /api/equipment/rented - Get rented equipment summary (Admin only)
        [HttpGet("rented")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<EquipmentListDto>>> GetRentedEquipmentSummary()
        {
            try
            {
                var rentedEquipment = await _equipmentService.GetRentedEquipment() ?? throw new KeyNotFoundException();
                var dtos = rentedEquipment.Select(e => new EquipmentListDto
                {
                    Id = e.Id.Value,
                    Name = e.Name,
                    Category = e.Category,
                    Condition = e.Condition,
                    RentalPrice = e.RentalPrice,
                    IsAvailable = e.IsAvailable,
                    ImageUrl = e.ImageUrl
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