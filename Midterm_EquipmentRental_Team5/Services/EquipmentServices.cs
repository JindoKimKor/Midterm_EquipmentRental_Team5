using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Services.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class EquipmentService : IEquipmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int PageSize = 10; // 10 items per page

        public EquipmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Equipment>> GetAllEquipmentAsync(int page = 1)
        {
            // Get all equipment from repository
            var allEquipment = _unitOfWork.Equipment.GetAllEquipment();

            // Calculate skip and take for pagination
            int skip = (page - 1) * PageSize;
            var paginatedEquipment = allEquipment.Skip(skip).Take(PageSize);

            return Task.FromResult(paginatedEquipment);
        }

        public Task<Equipment> GetEquipmentByIdAsync(int id)
        {
            // Get equipment by id from repository
            var equipment = _unitOfWork.Equipment.GetSpecificEquipment(id);

            return Task.FromResult(equipment);
        }

        public Task AddEquipmentAsync(Equipment newEquipment)
        {
            // Set creation timestamp
            newEquipment.CreatedAt = DateTime.Now;
            newEquipment.IsAvailable = true; // New equipment is available by default

            // Add equipment to repository
            _unitOfWork.Equipment.AddNewEquipment(newEquipment);
            
            // Save changes to database
            _unitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task UpdateEquipmentAsync(int id, Equipment updatedEquipment)
        {
            // Get existing equipment from repository
            var existingEquipment = _unitOfWork.Equipment.GetSpecificEquipment(id);

            if (existingEquipment == null)
            {
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            }

            // Update fields
            existingEquipment.Name = updatedEquipment.Name;
            existingEquipment.Description = updatedEquipment.Description;
            existingEquipment.Category = updatedEquipment.Category;
            existingEquipment.Condition = updatedEquipment.Condition;
            existingEquipment.RentalPrice = updatedEquipment.RentalPrice;
            existingEquipment.IsAvailable = updatedEquipment.IsAvailable;

            // Update in repository
            _unitOfWork.Equipment.UpdateEquipment(existingEquipment);
            
            // Save changes to database
            _unitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task DeleteEquipmentAsync(int id)
        {
            // Get equipment to delete
            var equipment = _unitOfWork.Equipment.GetSpecificEquipment(id);

            if (equipment == null)
            {
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            }

            // Delete from repository
            _unitOfWork.Equipment.DeleteEquipment(id);
            
            // Save changes to database
            _unitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Equipment>> GetAvailableEquipmentAsync()
        {
            // Get all equipment and filter by availability
            var allEquipment = _unitOfWork.Equipment.GetAllEquipment();
            var availableEquipment = allEquipment.Where(e => e.IsAvailable);

            return Task.FromResult(availableEquipment);
        }

        public Task<IEnumerable<Equipment>> GetRentedEquipmentAsync()
        {
            // Get all equipment and filter by rented status
            var allEquipment = _unitOfWork.Equipment.GetAllEquipment();
            var rentedEquipment = allEquipment.Where(e => !e.IsAvailable);

            return Task.FromResult(rentedEquipment);
        }
    }
}