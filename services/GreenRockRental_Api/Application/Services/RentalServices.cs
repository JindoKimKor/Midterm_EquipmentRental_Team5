using GreenRockRental_Api.Domain.Entities;
using GreenRockRental_Api.Application.DTOs;
using GreenRockRental_Api.Domain.Interfaces;
using GreenRockRental_Api.Application.Interfaces;
using GreenRockRental_Api.Infrastructure.UnitOfWork;

namespace GreenRockRental_Api.Application.Services
{
    public class RentalServices : IRentalServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int PageSize = 100;
        private const decimal OverdueFeePerDay = 10.00m;

        public RentalServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<IRental>?> GetAllRentals(int page = 1)
        {
            var allRentals = await _unitOfWork.Rentals.GetAllRentals();
            if (allRentals.Any())
            {
                int skip = (page - 1) * PageSize;
                allRentals = allRentals.Skip(skip).Take(PageSize);
            }
            return allRentals.Any() ? allRentals : null;
        }

        public async Task<IRental?> GetRentalById(int id)
        {
            var rental = await _unitOfWork.Rentals.GetRentalDetails(id);
            return rental ?? null;
        }

        public async Task IssueEquipment(IIssueRequest request)
        {
            var equipment = await _unitOfWork.Equipments.GetSpecificEquipment(request.EquipmentId) ?? throw new KeyNotFoundException($"Equipment with ID {request.EquipmentId} not found.");
            if (!equipment.IsAvailable)
            {
                throw new InvalidOperationException($"Equipment '{equipment.Name}' is not available.");
            }

            var customer = await _unitOfWork.Customers.GetCustomerDetails(request.CustomerId) ?? throw new KeyNotFoundException($"Customer with ID {request.CustomerId} not found.");
            var activeRental = await _unitOfWork.Customers.GetCustomerActiveRental(request.CustomerId);
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
            await _unitOfWork.Equipments.UpdateEquipment(equipment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ReturnEquipment(IReturnRequest request)
        {
            var rental = await _unitOfWork.Rentals.GetRentalDetails(request.RentalId) ?? throw new KeyNotFoundException($"Rental with ID {request.RentalId} not found.");
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

            var equipment = await _unitOfWork.Equipments.GetSpecificEquipment(rental.EquipmentId);
            if (equipment != null)
            {
                equipment.IsAvailable = true;
                equipment.Condition = request.Condition;
                await _unitOfWork.Equipments.UpdateEquipment(equipment);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<IRental>?> GetActiveRentals()
        {
            var activeRentals = await _unitOfWork.Rentals.GetActiveRentals();
            return activeRentals.Any() ? activeRentals : null;
        }

        public async Task<IEnumerable<IRental>?> GetCompletedRentals()
        {
            var completedRentals = await _unitOfWork.Rentals.GetCompletedRentals();
            return completedRentals.Any() ? completedRentals : null;
        }

        public async Task<IEnumerable<IRental>?> GetOverdueRentals()
        {
            var overdueRentals = await _unitOfWork.Rentals.GetOverdueRentals();
            return overdueRentals.Any() ? overdueRentals : null;
        }

        public async Task<IEnumerable<IRental>?> GetRentalHistoryByEquipment(int equipmentId)
        {
            var rentalHistory = await _unitOfWork.Rentals.GetEquipmentRentalHistory(equipmentId);
            return rentalHistory.Any() ? rentalHistory : null;
        }

        public async Task ExtendRental(int rentalId, IExtensionRequest request)
        {
            var rental = await _unitOfWork.Rentals.GetRentalDetails(rentalId) ?? throw new KeyNotFoundException($"Rental with ID {rentalId} not found.");
            if (rental.ReturnedAt.HasValue)
            {
                throw new InvalidOperationException("Cannot extend a completed rental.");
            }

            rental.DueDate = request.NewDueDate;
            rental.ExtensionReason = request.Reason;
            _unitOfWork.Rentals.UpdateRental(rental);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CancelRental(int rentalId)
        {
            var rental = await _unitOfWork.Rentals.GetRentalDetails(rentalId) ?? throw new KeyNotFoundException($"Rental with ID {rentalId} not found.");
            rental.ReturnedAt = DateTime.Now;
            rental.IsActive = false;

            if (rental.ReturnedAt > rental.DueDate)
            {
                int overdueDays = (rental.ReturnedAt.Value - rental.DueDate).Days;
                rental.OverdueFee = overdueDays * OverdueFeePerDay;
            }

            _unitOfWork.Rentals.UpdateRental(rental);

            var equipment = await _unitOfWork.Equipments.GetSpecificEquipment(rental.EquipmentId);
            if (equipment != null)
            {
                equipment.IsAvailable = true;
                await _unitOfWork.Equipments.UpdateEquipment(equipment);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
