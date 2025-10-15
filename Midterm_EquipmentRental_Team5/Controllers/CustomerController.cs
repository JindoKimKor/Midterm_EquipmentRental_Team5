using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Services;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerServices _customerService;

        public CustomerController(CustomerServices customerServices)
        {
            _customerService = customerServices;
        }

        // GET /api/customers
        [HttpGet]
        public ActionResult<IEnumerable<object>> GetAllCustomers(int page = 1)
        {
            // TODO: Return paginated customers list
            return Ok();
        }

        // GET /api/customers/{id}
        [HttpGet("{id}")]
        public ActionResult<object> GetCustomer(int id)
        {
            // TODO: Return customer details (admin sees all, user sees own data)
            return Ok();
        }

        // POST /api/customers
        [HttpPost]
        public ActionResult CreateCustomer([FromBody] object newCustomer)
        {
            // TODO: Create new customer
            return CreatedAtAction(nameof(GetCustomer), new { id = 0 }, null);
        }

        // PUT /api/customers/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCustomer(int id, [FromBody] object updatedCustomer)
        {
            // TODO: Update customer (admin can edit role, user can edit name/password)
            return NoContent();
        }

        // DELETE /api/customers/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCustomer(int id)
        {
            // TODO: Delete customer and rental history with confirmation
            return NoContent();
        }

        // GET /api/customers/{id}/rentals
        [HttpGet("{id}/rentals")]
        public ActionResult<IEnumerable<object>> GetCustomerRentalHistory(int id)
        {
            // TODO: Return rental history for customer id
            return Ok();
        }

        // GET /api/customers/{id}/active-rental
        [HttpGet("{id}/active-rental")]
        public ActionResult<object> GetCustomerActiveRental(int id)
        {
            // TODO: Return active rental info for dashboard
            return Ok();
        }

    }
}
