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
    public class EquipmentController(IEquipmentServices equipmentService, IWebHostEnvironment environment) : ControllerBase
    {
        private readonly IEquipmentServices _equipmentService = equipmentService;
        // GET /api/equipment - Get all equipment with pagination
        [HttpGet]
        public ActionResult<IEnumerable<IEquipment>> GetAllEquipment(int page = 1)
        {
            try
            {
                var equipment = equipmentService.GetAllEquipmentAsync(page);
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
        
        [HttpPost("{id}/upload-image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No image file provided");

            // ✅ Added await
            var equipment = equipmentService.GetEquipmentByIdAsync(id);
            if (equipment == null)
                return NotFound("Equipment not found");
            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Invalid file type. Only jpg, jpeg, png, gif allowed");

            // Create unique filename
            var fileName = $"equipment_{id}_{Guid.NewGuid()}{extension}";
            var uploadsFolder = Path.Combine(environment.WebRootPath, "images", "equipment");

            // Create directory if it doesn't exist
            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save file - use await using
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            // ImageUrl will work since it's on the interface
            equipment.ImageUrl = $"/images/equipment/{fileName}";
            equipmentService.UpdateEquipmentAsync(id, equipment);

            return Ok(new { imageUrl = equipment.ImageUrl });
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

        // GET /api/equipment/rented - Get rented equipment summary (Admin only)i
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