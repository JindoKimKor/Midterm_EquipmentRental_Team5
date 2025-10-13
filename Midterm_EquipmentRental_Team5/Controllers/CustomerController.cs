using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // TODO: Implement 7 endpoints
        // TODO: GET /api/customers - List all customers [Authorize(Roles="Admin")]
        // TODO: GET /api/customers/{id} - Get customer details [Authorize]
        // TODO: POST /api/customers - Create customer [Authorize(Roles="Admin")]
        // TODO: PUT /api/customers/{id} - Update customer [Authorize]
        // TODO: DELETE /api/customers/{id} - Delete customer [Authorize(Roles="Admin")]
        // TODO: GET /api/customers/{id}/rentals - Get customer rental history [Authorize]
        // TODO: GET /api/customers/{id}/active-rental - Get customer active rental [Authorize]

    }
}
