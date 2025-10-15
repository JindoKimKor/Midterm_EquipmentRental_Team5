using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Services.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class EquipmentService : IEquipmentServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public EquipmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Equipment>> GetAllEquipmentAsync(int page = 1)
        {
            // TODO: Return paginated list of all equipment
            throw new NotImplementedException();
        }

        public Task<Equipment> GetEquipmentByIdAsync(int id)
        {
            // TODO: Return equipment details by id
            throw new NotImplementedException();
        }

        public Task AddEquipmentAsync(Equipment newEquipment)
        {
            // TODO: Add new equipment
            throw new NotImplementedException();
        }

        public Task UpdateEquipmentAsync(int id, Equipment updatedEquipment)
        {
            // TODO: Update equipment by id
            throw new NotImplementedException();
        }

        public Task DeleteEquipmentAsync(int id)
        {
            // TODO: Delete equipment by id
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Equipment>> GetAvailableEquipmentAsync()
        {
            // TODO: Return list of available equipment
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Equipment>> GetRentedEquipmentAsync()
        {
            // TODO: Return list of rented equipment
            throw new NotImplementedException();
        }
    }

    public interface IEquipmentService
    {
    }
}
