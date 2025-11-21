using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;
using Midterm_EquipmentRental_Team5.Application.Interfaces;

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
        public ActionResult<IEnumerable<IEquipment>> GetAllEquipment(int page = 1)
        {
            try
            {
                var equipment = equipmentService.GetAllEquipment(page) ?? throw new KeyNotFoundException();
                return Ok(equipment);
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
        public ActionResult<IEnumerable<IRental>> GetEquipmentRentalHistory(int id)
        {
            try
            {
                // First check if equipment exists
                var equipment = _equipmentService.GetEquipmentById(id);
                if (equipment == null)
                {
                    return NotFound($"Equipment with ID {id} not found.");
                }

                // Get rental history for this equipment
                var rentalHistory = _rentalService.GetRentalHistoryByEquipment(id);
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



        // GET /api/equipment/{id} - Get specific equipment details
        [HttpGet("{id}")]
        public ActionResult<IEquipment> GetEquipment(int id)
        {
            try
            {
                var equipment = _equipmentService.GetEquipmentById(id) ?? throw new KeyNotFoundException();
                return Ok(equipment);
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
        public ActionResult AddEquipment([FromBody] Equipment newEquipment)
        {
            try
            {
                _equipmentService.AddEquipment(newEquipment);
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
                _equipmentService.UpdateEquipment(id, updatedEquipment);
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
        public ActionResult DeleteEquipment(int id)
        {
            try
            {
                _equipmentService.DeleteEquipment(id);
                return NoContent();
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
                var availableEquipment = _equipmentService.GetAvailableEquipment() ?? throw new KeyNotFoundException();
                return Ok(availableEquipment);
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
        public ActionResult<IEquipment> GetRentedEquipmentSummary()
        {
            try
            {
                var rentedEquipment = _equipmentService.GetRentedEquipment() ?? throw new KeyNotFoundException();
                return Ok(rentedEquipment);
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