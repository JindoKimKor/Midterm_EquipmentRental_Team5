using GreenRockRental_Api.Domain.Entities;
using GreenRockRental_Api.Domain.Interfaces;
using GreenRockRental_Api.Application.Interfaces;
using GreenRockRental_Api.Infrastructure.UnitOfWork;

namespace GreenRockRental_Api.Application.Services
{
    public class EquipmentService : IEquipmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int PageSize = 100;

        public EquipmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<IEquipment>?> GetAllEquipment(int page = 1)
        {
            var allEquipment = await _unitOfWork.Equipments.GetAllEquipment();
            if (allEquipment.Any())
            {
                // pagnation
                var skip = (page - 1) * PageSize;
                allEquipment = allEquipment.Skip(skip).Take(PageSize);
            }
            return allEquipment ?? null;
        }

        public async Task<IEquipment?> GetEquipmentById(int id)
        {
            var equipment = await _unitOfWork.Equipments.GetSpecificEquipment(id);
            return equipment;
        }

        public async Task AddEquipment(IEquipment newEquipment)
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
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateEquipment(int id, IEquipment updatedEquipment)
        {
            var existingEquipment = await _unitOfWork.Equipments.GetSpecificEquipment(id) ?? throw new KeyNotFoundException();
            if (existingEquipment != null)
            {
                existingEquipment.Name = updatedEquipment.Name;
                existingEquipment.Description = updatedEquipment.Description;
                existingEquipment.Category = updatedEquipment.Category;
                existingEquipment.Condition = updatedEquipment.Condition;
                existingEquipment.RentalPrice = updatedEquipment.RentalPrice;
                existingEquipment.IsAvailable = updatedEquipment.IsAvailable;
                await _unitOfWork.Equipments.UpdateEquipment(existingEquipment);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteEquipment(int id)
        {
            var existingEquipment = await _unitOfWork.Equipments.GetSpecificEquipment(id) ?? throw new KeyNotFoundException();
            if (existingEquipment != null)
            {
                await _unitOfWork.Equipments.DeleteEquipment((int)(existingEquipment.Id ?? 0));
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<IEquipment>?> GetAvailableEquipment()
        {
            var equipments = await _unitOfWork.Equipments.ListAvailableEquipment();
            return equipments.Any() ? equipments : null;
        }

        public async Task<IEnumerable<IEquipment>?> GetRentedEquipment()
        {
            var equipments = await _unitOfWork.Equipments.GetRentedEquipment();
            return equipments.Any() ? equipments : null;
        }
    }
}
