using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Domain.Interfaces;
using Midterm_EquipmentRental_Team5.Application.Interfaces;
using Midterm_EquipmentRental_Team5.Infrastructure.UnitOfWork;

namespace Midterm_EquipmentRental_Team5.Application.Services
{
    public class EquipmentService : IEquipmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int PageSize = 100;

        public EquipmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<IEquipment>? GetAllEquipment(int page = 1)
        {
            var allEquipment = _unitOfWork.Equipments.GetAllEquipment();
            if (allEquipment.Any())
            {
                // pagnation
                var skip = (page - 1) * PageSize;
                allEquipment = allEquipment.Skip(skip).Take(PageSize);
            }
            return allEquipment ?? null;
        }

        public IEquipment GetEquipmentById(int id)
        {
            var equipment = _unitOfWork.Equipments.GetSpecificEquipment(id);
            return equipment;
        }

        public void AddEquipment(IEquipment newEquipment)
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

        public void UpdateEquipment(int id, IEquipment updatedEquipment)
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
        }

        public void DeleteEquipment(int id)
        {
            var existingEquipment = _unitOfWork.Equipments.GetSpecificEquipment(id) ?? throw new KeyNotFoundException();
            if (existingEquipment != null)
            {
                _unitOfWork.Equipments.DeleteEquipment((int)existingEquipment.Id);
                _unitOfWork.SaveChanges();
            }
        }

        public IEnumerable<IEquipment>? GetAvailableEquipment()
        {
            var equipments = _unitOfWork.Equipments.ListAvailableEquipment();
            return equipments.Any() ? equipments : null;
        }

        public IEnumerable<IEquipment>? GetRentedEquipment()
        {
            var equipments = _unitOfWork.Equipments.GetRentedEquipment();
            return equipments.Any() ? equipments : null;
        }
    }
}
