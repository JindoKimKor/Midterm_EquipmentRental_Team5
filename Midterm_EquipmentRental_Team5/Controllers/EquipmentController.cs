using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Services;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly EquipmentService _equipmentService;

        public EquipmentController(EquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        // GET /api/equipment
        [HttpGet]
        public ActionResult<IEnumerable<object>> GetAllEquipment(int page = 1)
        {
            // TODO: Return paginated list
            return Ok();
        }

        // GET /api/equipment/{id}
        [HttpGet("{id}")]
        public ActionResult<object> GetEquipment(int id)
        {
            // TODO: Return details for equipment id
            return Ok();
        }

        // POST /api/equipment
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddEquipment([FromBody] object newEquipment)
        {
            // TODO: Add new equipment
            return CreatedAtAction(nameof(GetEquipment), new { id = 0 }, null);
        }

        // PUT /api/equipment/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateEquipment(int id, [FromBody] object updatedEquipment)
        {
            // TODO: Update equipment by id
            return NoContent();
        }

        // DELETE /api/equipment/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteEquipment(int id)
        {
            // TODO: Delete equipment by id
            return NoContent();
        }

        // GET /api/equipment/available
        [HttpGet("available")]
        public ActionResult<IEnumerable<object>> GetAvailableEquipment()
        {
            // TODO: Return list of available equipment
            return Ok();
        }

        // GET /api/equipment/rented
        [HttpGet("rented")]
        [Authorize(Roles = "Admin")]
        public ActionResult<object> GetRentedEquipmentSummary()
        {
            // TODO: Return rented equipment summary
            return Ok();
        }

    }
}
