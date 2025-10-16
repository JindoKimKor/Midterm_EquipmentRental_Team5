using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Services;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // All endpoints require authentication
    public class EquipmentController : ControllerBase
    {
        private readonly EquipmentService _equipmentService;

        public EquipmentController(EquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        // GET /api/equipment - Get all equipment with pagination
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllEquipment(int page = 1)
        {
            try
            {
                var equipment = await _equipmentService.GetAllEquipmentAsync(page);
                return Ok(equipment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/equipment/{id} - Get specific equipment details
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetEquipment(int id)
        {
            try
            {
                var equipment = await _equipmentService.GetEquipmentByIdAsync(id);
                
                if (equipment == null)
                {
                    return NotFound($"Equipment with ID {id} not found.");
                }

                return Ok(equipment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /api/equipment - Add new equipment (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddEquipment([FromBody] Equipment newEquipment)
        {
            try
            {
                await _equipmentService.AddEquipmentAsync(newEquipment);
                return CreatedAtAction(nameof(GetEquipment), new { id = newEquipment.Id }, newEquipment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/equipment/{id} - Update equipment (Admin only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateEquipment(int id, [FromBody] Equipment updatedEquipment)
        {
            try
            {
                await _equipmentService.UpdateEquipmentAsync(id, updatedEquipment);
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

        // DELETE /api/equipment/{id} - Delete equipment (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteEquipment(int id)
        {
            try
            {
                await _equipmentService.DeleteEquipmentAsync(id);
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

        // GET /api/equipment/available - Get list of available equipment
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<object>>> GetAvailableEquipment()
        {
            try
            {
                var availableEquipment = await _equipmentService.GetAvailableEquipmentAsync();
                return Ok(availableEquipment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/equipment/rented - Get rented equipment summary (Admin only)
        [HttpGet("rented")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<object>> GetRentedEquipmentSummary()
        {
            try
            {
                var rentedEquipment = await _equipmentService.GetRentedEquipmentAsync();
                
                // Create summary response
                var summary = new
                {
                    TotalRented = rentedEquipment.Count(),
                    Equipment = rentedEquipment
                };

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}