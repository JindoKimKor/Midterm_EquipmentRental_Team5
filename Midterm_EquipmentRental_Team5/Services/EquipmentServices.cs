using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;
using Midterm_EquipmentRental_Team5.Services.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class EquipmentService : IEquipmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int PageSize = 10;

        public EquipmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<IEquipment>?> GetAllEquipmentAsync(int page = 1)
        {
            var allEquipment = _unitOfWork.Equipments.GetAllEquipment() ?? throw new KeyNotFoundException();
            if (allEquipment != null)
            {
                // pagnation
                var skip = (page - 1) * PageSize;
                allEquipment = allEquipment.Skip(skip).Take(PageSize);
            }
            return Task.FromResult(allEquipment);
        }

        public Task<IEquipment> GetEquipmentByIdAsync(int id)
        {
            var equipment = _unitOfWork.Equipments.GetSpecificEquipment(id) ?? throw new KeyNotFoundException();
            return Task.FromResult(equipment);
        }

        public Task AddEquipmentAsync(IEquipment newEquipment)
        {
            _unitOfWork.Equipments.AddNewEquipment(new Equipment()
            {
                IsAvailable = newEquipment.IsAvailable,
                Name = newEquipment.Name,
                Description = newEquipment.Description,
                Category = newEquipment.Category,
                Condition = newEquipment.Condition,
                RentalPrice = newEquipment.RentalPrice,
                CreatedAt = newEquipment.CreatedAt
            });
            _unitOfWork.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<IEquipment?> UpdateEquipmentAsync(int id, IEquipment updatedEquipment)
        {
            var existingEquipment = _unitOfWork.Equipments.GetSpecificEquipment(id) ?? throw new KeyNotFoundException();
            if (existingEquipment != null)
            {
                existingEquipment.Name = updatedEquipment.Name;
                existingEquipment.Description = updatedEquipment.Description;
                existingEquipment.Category = updatedEquipment.Category;
                existingEquipment.Condition = updatedEquipment.Condition;
                existingEquipment.RentalPrice = updatedEquipment.RentalPrice;
                existingEquipment.IsAvailable = updatedEquipment.IsAvailable;
                _unitOfWork.Equipments.UpdateEquipment(existingEquipment);
                _unitOfWork.SaveChanges();
            }
            return Task.FromResult(existingEquipment);
        }

        public Task DeleteEquipmentAsync(int id)
        {
            var equipment = _unitOfWork.Equipments.GetSpecificEquipment(id) ?? throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            _unitOfWork.Equipments.DeleteEquipment(id);
            _unitOfWork.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<IEquipment>?> GetAvailableEquipmentAsync()
        {
            return Task.FromResult(_unitOfWork.Equipments.ListAvailableEquipment() ?? throw new KeyNotFoundException());
        }

        public Task<IEnumerable<IEquipment>?> GetRentedEquipmentAsync()
        {
            return Task.FromResult(_unitOfWork.Equipments.GetRentedEquipment() ?? throw new KeyNotFoundException());
        }
    }
}
