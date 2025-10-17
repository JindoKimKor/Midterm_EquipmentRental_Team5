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

        public Task<IEnumerable<IEquipment>> GetAllEquipmentAsync(int page = 1)
        {
            var allEquipment = _unitOfWork.Equipments.GetAllEquipment();
            int skip = (page - 1) * PageSize;
            var paginatedEquipment = allEquipment.Skip(skip).Take(PageSize);
            return Task.FromResult(paginatedEquipment);
        }

        public Task<IEquipment> GetEquipmentByIdAsync(int id)
        {
            var equipment = _unitOfWork.Equipments.GetSpecificEquipment(id);
            return Task.FromResult(equipment);
        }

        public Task AddEquipmentAsync(IEquipment newEquipment)
        {
            newEquipment.CreatedAt = DateTime.Now;
            newEquipment.IsAvailable = true;
            _unitOfWork.Equipments.AddNewEquipment(newEquipment);
            _unitOfWork.SaveChanges();
            return Task.CompletedTask;
        }

        public Task UpdateEquipmentAsync(int id, IEquipment updatedEquipment)
        {
            var existingEquipment = _unitOfWork.Equipments.GetSpecificEquipment(id);
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
            return Task.CompletedTask;
        }

        public Task DeleteEquipmentAsync(int id)
        {
            var equipment = _unitOfWork.Equipments.GetSpecificEquipment(id) ?? throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            _unitOfWork.Equipments.DeleteEquipment(id);
            _unitOfWork.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<IEquipment>> GetAvailableEquipmentAsync()
        {
            var availableEquipment = _unitOfWork.Equipments.ListAvailableEquipment();
            return Task.FromResult(availableEquipment);
        }

        public Task<IEnumerable<IEquipment>> GetRentedEquipmentAsync()
        {
            var rentedEquipment = _unitOfWork.Equipments.GetRentedEquipment();
            return Task.FromResult(rentedEquipment);
        }
    }
}
