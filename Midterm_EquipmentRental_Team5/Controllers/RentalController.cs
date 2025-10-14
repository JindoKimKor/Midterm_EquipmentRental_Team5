using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Services;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly RentalServices _rentalService;

        public RentalController(RentalServices rentalService)
        {
            _rentalService = rentalService;
        }

        // GET /api/rentals
        [HttpGet]
        public ActionResult<IEnumerable<object>> GetAllRentals(int page = 1)
        {
            // TODO: Admin sees all, user sees own rentals
            return Ok();
        }

        // GET /api/rentals/{id}
        [HttpGet("{id}")]
        public ActionResult<object> GetRentalDetails(int id)
        {
            // TODO: Return rental details card
            return Ok();
        }

        // POST /api/rentals/issue
        [HttpPost("issue")]
        [Authorize(Roles = "Admin")]
        public ActionResult IssueEquipment([FromBody] object issueRequest)
        {
            // TODO: Issue equipment to customer
            return Ok();
        }

        // POST /api/rentals/return
        [HttpPost("return")]
        public ActionResult ReturnEquipment([FromBody] object returnRequest)
        {
            // TODO: Return equipment form submission
            return Ok();
        }

        // GET /api/rentals/active
        [HttpGet("active")]
        public ActionResult<IEnumerable<object>> GetActiveRentals()
        {
            // TODO: Return grid of active rentals
            return Ok();
        }

        // GET /api/rentals/completed
        [HttpGet("completed")]
        public ActionResult<IEnumerable<object>> GetCompletedRentals()
        {
            // TODO: Return table of completed rentals
            return Ok();
        }

        // GET /api/rentals/overdue
        [HttpGet("overdue")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<object>> GetOverdueRentals()
        {
            // TODO: Return alert table of overdue rentals
            return Ok();
        }

        // GET /api/rentals/equipment/{equipmentId}
        [HttpGet("equipment/{equipmentId}")]
        public ActionResult<IEnumerable<object>> GetEquipmentRentalHistory(int equipmentId)
        {
            // TODO: Return rental timeline for equipment
            return Ok();
        }

        // PUT /api/rentals/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult ExtendRental(int id, [FromBody] object extensionRequest)
        {
            // TODO: Extend rental with due date and reason
            return NoContent();
        }

        // DELETE /api/rentals/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult CancelRental(int id)
        {
            // TODO: Force return rental, with confirmation modal on frontend
            return NoContent();
        }
    }
}
