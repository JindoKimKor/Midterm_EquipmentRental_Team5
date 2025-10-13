using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EquipmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // TODO: Implement 7 endpoints
        // TODO: GET /api/equipment - Get all equipment
        // TODO: GET /api/equipment/{id} - Get specific equipment
        // TODO: POST /api/equipment - Add new equipment [Authorize(Roles="Admin")]
        // TODO: PUT /api/equipment/{id} - Update equipment [Authorize(Roles="Admin")]
        // TODO: DELETE /api/equipment/{id} - Delete equipment [Authorize(Roles="Admin")]
        // TODO: GET /api/equipment/available - List available equipment
        // TODO: GET /api/equipment/rented - Get rented equipment [Authorize(Roles="Admin")]

    }
}
