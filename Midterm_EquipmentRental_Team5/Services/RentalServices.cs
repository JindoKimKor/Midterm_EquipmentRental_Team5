using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.DTOs;
using Midterm_EquipmentRental_Team5.Models.Interfaces;
using Midterm_EquipmentRental_Team5.Services.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class RentalServices : IRentalServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int PageSize = 10;
        private const decimal OverdueFeePerDay = 10.00m;

        public RentalServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<IRental>> GetAllRentalsAsync(int page = 1)
        {
            var allRentals = _unitOfWork.Rentals.GetAllRentals();
            int skip = (page - 1) * PageSize;
            var paginatedRentals = allRentals.Skip(skip).Take(PageSize);
            return Task.FromResult(paginatedRentals);
        }

        public Task<IRental> GetRentalByIdAsync(int id)
        {
            var rental = _unitOfWork.Rentals.GetRentalDetails(id);
            return Task.FromResult(rental);
        }

        public Task IssueEquipmentAsync(IIssueRequest request)
        {
            var equipment = _unitOfWork.Equipments.GetSpecificEquipment(request.EquipmentId) ?? throw new KeyNotFoundException($"Equipment with ID {request.EquipmentId} not found.");
            if (!equipment.IsAvailable)
            {
                throw new InvalidOperationException($"Equipment '{equipment.Name}' is not available.");
            }

            var customer = _unitOfWork.Customers.GetCustomerDetails(request.CustomerId) ?? throw new KeyNotFoundException($"Customer with ID {request.CustomerId} not found.");
            var activeRental = _unitOfWork.Customers.GetCustomerActiveRental(request.CustomerId);
            if (activeRental != null)
            {
                throw new InvalidOperationException($"Customer '{customer.Name}' already has an active rental.");
            }

            var newRental = new Rental
            {
                EquipmentId = request.EquipmentId,
                CustomerId = request.CustomerId,
                IssuedAt = DateTime.Now,
                DueDate = request.DueDate ?? DateTime.Now.AddDays(7),
                ReturnedAt = null,
                IsActive = true
            };

            _unitOfWork.Rentals.IssueEquipment(newRental, newRental.DueDate);
            equipment.IsAvailable = false;
            _unitOfWork.Equipments.UpdateEquipment(equipment);
            _unitOfWork.SaveChanges();
            return Task.CompletedTask;
        }

        public Task ReturnEquipmentAsync(IReturnRequest request)
        {
            var rental = _unitOfWork.Rentals.GetRentalDetails(request.RentalId) ?? throw new KeyNotFoundException($"Rental with ID {request.RentalId} not found.");
            if (rental.ReturnedAt.HasValue)
            {
                throw new InvalidOperationException("This rental has already been returned.");
            }

            rental.ReturnedAt = DateTime.Now;
            rental.IsActive = false;

            if (rental.ReturnedAt > rental.DueDate)
            {
                int overdueDays = (rental.ReturnedAt.Value - rental.DueDate).Days;
                rental.OverdueFee = overdueDays * OverdueFeePerDay;
            }

            _unitOfWork.Rentals.UpdateRental(rental);

            var equipment = _unitOfWork.Equipments.GetSpecificEquipment(rental.EquipmentId);
            if (equipment != null)
            {
                equipment.IsAvailable = true;
                equipment.Condition = request.Condition;
                _unitOfWork.Equipments.UpdateEquipment(equipment);
            }

            _unitOfWork.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<IRental>> GetActiveRentalsAsync()
        {
            var activeRentals = _unitOfWork.Rentals.GetActiveRentals();
            return Task.FromResult(activeRentals);
        }

        public Task<IEnumerable<IRental>> GetCompletedRentalsAsync()
        {
            var completedRentals = _unitOfWork.Rentals.GetCompletedRentals();
            return Task.FromResult(completedRentals);
        }

        public Task<IEnumerable<IRental>> GetOverdueRentalsAsync()
        {
            var overdueRentals = _unitOfWork.Rentals.GetOverdueRentals();
            return Task.FromResult(overdueRentals);
        }

        public Task<IEnumerable<IRental>> GetRentalHistoryByEquipmentAsync(int equipmentId)
        {
            var rentalHistory = _unitOfWork.Rentals.GetEquipmentRentalHistory(equipmentId);
            return Task.FromResult(rentalHistory);
        }

        public Task ExtendRentalAsync(int rentalId, IExtensionRequest request)
        {
            var rental = _unitOfWork.Rentals.GetRentalDetails(rentalId);
            if (rental == null)
            {
                throw new KeyNotFoundException($"Rental with ID {rentalId} not found.");
            }
            if (rental.ReturnedAt.HasValue)
            {
                throw new InvalidOperationException("Cannot extend a completed rental.");
            }

            rental.DueDate = request.NewDueDate;
            rental.ExtensionReason = request.Reason;
            _unitOfWork.Rentals.UpdateRental(rental);
            _unitOfWork.SaveChanges();
            return Task.CompletedTask;
        }

        public Task CancelRentalAsync(int rentalId)
        {
            var rental = _unitOfWork.Rentals.GetRentalDetails(rentalId) ?? throw new KeyNotFoundException($"Rental with ID {rentalId} not found.");
            rental.ReturnedAt = DateTime.Now;
            rental.IsActive = false;

            if (rental.ReturnedAt > rental.DueDate)
            {
                int overdueDays = (rental.ReturnedAt.Value - rental.DueDate).Days;
                rental.OverdueFee = overdueDays * OverdueFeePerDay;
            }

            _unitOfWork.Rentals.UpdateRental(rental);

            var equipment = _unitOfWork.Equipments.GetSpecificEquipment(rental.EquipmentId);
            if (equipment != null)
            {
                equipment.IsAvailable = true;
                _unitOfWork.Equipments.UpdateEquipment(equipment);
            }

            _unitOfWork.SaveChanges();
            return Task.CompletedTask;
        }
    }
}