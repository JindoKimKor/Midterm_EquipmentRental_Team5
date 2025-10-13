using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // TODO: Implement 10 endpoints
        // TODO: GET /api/rentals - Get all rentals [Authorize]
        // TODO: GET /api/rentals/{id} - Get rental details [Authorize]
        // TODO: POST /api/rentals/issue - Issue equipment [Authorize]
        // TODO: POST /api/rentals/return - Return equipment [Authorize]
        // TODO: GET /api/rentals/active - Get active rentals [Authorize]
        // TODO: GET /api/rentals/completed - Get completed rentals [Authorize]
        // TODO: GET /api/rentals/overdue - Get overdue rentals [Authorize(Roles="Admin")]
        // TODO: GET /api/rentals/equipment/{equipmentId} - Equipment rental history
        // TODO: PUT /api/rentals/{id} - Extend rental [Authorize]
        // TODO: DELETE /api/rentals/{id} - Cancel rental [Authorize(Roles="Admin")]

    }
}
