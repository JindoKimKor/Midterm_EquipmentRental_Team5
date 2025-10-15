using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Services.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class RentalServices : IRentalServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Rental>> GetAllRentalsAsync(int page = 1)
        {
            // TODO: Return all rentals, paginated
            throw new NotImplementedException();
        }

        public Task<Rental> GetRentalByIdAsync(int id)
        {
            // TODO: Return rental details by id
            throw new NotImplementedException();
        }

        // public Task IssueEquipmentAsync(IssueRequest request)
        // {
        //     // TODO: Logic to issue equipment
        //     throw new NotImplementedException();
        // }

        // public Task ReturnEquipmentAsync(ReturnRequest request)
        // {
        //     // TODO: Logic to process equipment return
        //     throw new NotImplementedException();
        // }

        public Task<IEnumerable<Rental>> GetActiveRentalsAsync()
        {
            // TODO: Return active rentals
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Rental>> GetCompletedRentalsAsync()
        {
            // TODO: Return completed rentals
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Rental>> GetOverdueRentalsAsync()
        {
            // TODO: Return overdue rentals
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Rental>> GetRentalHistoryByEquipmentAsync(int equipmentId)
        {
            // TODO: Return rental history for given equipment
            throw new NotImplementedException();
        }

        // public Task ExtendRentalAsync(int rentalId, ExtensionRequest request)
        // {
        //     // TODO: Extend rental
        //     throw new NotImplementedException();
        // }

        public Task CancelRentalAsync(int rentalId)
        {
            // TODO: Cancel (force return) rental
            throw new NotImplementedException();
        }
    }
}
