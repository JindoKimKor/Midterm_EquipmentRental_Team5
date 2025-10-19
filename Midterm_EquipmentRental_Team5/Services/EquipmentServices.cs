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

        public IEnumerable<IEquipment>? GetAllEquipmentAsync(int page = 1)
        {
            var allEquipment = _unitOfWork.Equipments.GetAllEquipment() ?? throw new KeyNotFoundException();
            if (allEquipment != null)
            {
                // pagnation
                var skip = (page - 1) * PageSize;
                allEquipment = allEquipment.Skip(skip).Take(PageSize);
            }
            return allEquipment;
        }

        public IEquipment GetEquipmentByIdAsync(int id)
        {
            var equipment = _unitOfWork.Equipments.GetSpecificEquipment(id) ?? throw new KeyNotFoundException();
            return equipment;
        }

        public void AddEquipmentAsync(IEquipment newEquipment)
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
        }

        public IEquipment? UpdateEquipmentAsync(int id, IEquipment updatedEquipment)
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
            return existingEquipment;
        }

        public void DeleteEquipmentAsync(int id)
        {
            var equipment = _unitOfWork.Equipments.GetSpecificEquipment(id) ?? throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            _unitOfWork.Equipments.DeleteEquipment(id);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<IEquipment>? GetAvailableEquipmentAsync()
        {
            return _unitOfWork.Equipments.ListAvailableEquipment() ?? throw new KeyNotFoundException();
        }

        public IEnumerable<IEquipment>? GetRentedEquipmentAsync()
        {
            return _unitOfWork.Equipments.GetRentedEquipment() ?? throw new KeyNotFoundException();
        }
    }
}
