using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;
using Midterm_EquipmentRental_Team5.Services;
using Midterm_EquipmentRental_Team5.Services.Interfaces;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // All endpoints require authentication
    public class EquipmentController(IEquipmentServices equipmentService) : ControllerBase
    {
        private readonly IEquipmentServices _equipmentService = equipmentService;

        // GET /api/equipment - Get all equipment with pagination
        [HttpGet]
        public ActionResult<IEnumerable<IEquipment>> GetAllEquipment(int page = 1)
        {
            try
            {
                var equipment = _equipmentService.GetAllEquipmentAsync(page);
                return Ok(equipment);
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
        public ActionResult<IEquipment> GetEquipment(int id)
        {
            try
            {
                var equipment = _equipmentService.GetEquipmentByIdAsync(id);

                if (equipment == null)
                {
                    return NotFound($"Equipment with ID {id} not found.");
                }

                return Ok(equipment);
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

        // POST /api/equipment - Add new equipment (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddEquipment([FromBody] Equipment newEquipment)
        {
            try
            {
               _equipmentService.AddEquipmentAsync(newEquipment);
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
        public ActionResult UpdateEquipment(int id, [FromBody] Equipment updatedEquipment)
        {
            try
            {
                _equipmentService.UpdateEquipmentAsync(id, updatedEquipment);
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
        public ActionResult DeleteEquipment(int id)
        {
            try
            {
                _equipmentService.DeleteEquipmentAsync(id);
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
        public ActionResult<IEnumerable<IEquipment>> GetAvailableEquipment()
        {
            try
            {
                var availableEquipment = _equipmentService.GetAvailableEquipmentAsync();
                return Ok(availableEquipment);
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

        // GET /api/equipment/rented - Get rented equipment summary (Admin only)
        [HttpGet("rented")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEquipment> GetRentedEquipmentSummary()
        {
            try
            {
                var rentedEquipment = _equipmentService.GetRentedEquipmentAsync();
                return Ok(rentedEquipment);
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