using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.DTOs;
using Midterm_EquipmentRental_Team5.Services.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.Services
{
    public class RentalServices : IRentalServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int PageSize = 10; // 10 items a page
        private const decimal OverdueFeePerDay = 10.00m; // $10 per day overdue

        public RentalServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Rental>> GetAllRentalsAsync(int page = 1)
        {
            // Get all rentals from repository
            var allRentals = _unitOfWork.Rentals.GetAllRentals();

            // Calculate skip and take for pagination
            int skip = (page - 1) * PageSize;
            var paginatedRentals = allRentals.Skip(skip).Take(PageSize);

            return Task.FromResult(paginatedRentals);
        }

        public Task<Rental> GetRentalByIdAsync(int id)
        {
            // Get rental by id from repository
            var rental = _unitOfWork.Rentals.GetRentalDetails(id);

            return Task.FromResult(rental);
        }

        public Task IssueEquipmentAsync(IssueRequest request)
        {
            // Validate: Check if equipment exists and is available
            var equipment = _unitOfWork.Equipment.GetSpecificEquipment(request.EquipmentId);
            if (equipment == null)
            {
                throw new KeyNotFoundException($"Equipment with ID {request.EquipmentId} not found.");
            }

            if (!equipment.IsAvailable)
            {
                throw new InvalidOperationException($"Equipment '{equipment.Name}' is not available.");
            }

            // Validate: Check if customer exists
            var customer = _unitOfWork.Customers.GetCustomerDetails(request.CustomerId);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {request.CustomerId} not found.");
            }

            // Validate: Check if customer already has an active rental
            var activeRental = _unitOfWork.Rentals.GetAllRentals()
                .FirstOrDefault(r => r.CustomerId == request.CustomerId && r.ReturnedAt == null);

            if (activeRental != null)
            {
                throw new InvalidOperationException($"Customer '{customer.Name}' already has an active rental.");
            }

            // Create new rental
            var newRental = new Rental
            {
                EquipmentId = request.EquipmentId,
                CustomerId = request.CustomerId,
                IssuedAt = DateTime.Now,
                DueDate = request.DueDate ?? DateTime.Now.AddDays(7), // Default 7 days if not specified
                ReturnedAt = null
            };

            // Add rental to repository
            _unitOfWork.Rentals.CreateRental(newRental);

            // Update equipment availability
            equipment.IsAvailable = false;
            _unitOfWork.Equipment.UpdateEquipment(equipment);

            // Save changes to database
            _unitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task ReturnEquipmentAsync(ReturnRequest request)
        {
            // Get rental by id
            var rental = _unitOfWork.Rentals.GetRentalDetails(request.RentalId);
            if (rental == null)
            {
                throw new KeyNotFoundException($"Rental with ID {request.RentalId} not found.");
            }

            if (rental.ReturnedAt.HasValue)
            {
                throw new InvalidOperationException("This rental has already been returned.");
            }

            // Update rental with return timestamp
            rental.ReturnedAt = DateTime.Now;

            // Calculate overdue fee if applicable
            if (rental.ReturnedAt > rental.DueDate)
            {
                int overdueDays = (rental.ReturnedAt.Value - rental.DueDate).Days;
                rental.OverdueFee = overdueDays * OverdueFeePerDay;
            }

            _unitOfWork.Rentals.UpdateRental(rental);

            // Update equipment availability and condition
            var equipment = _unitOfWork.Equipment.GetSpecificEquipment(rental.EquipmentId);
            if (equipment != null)
            {
                equipment.IsAvailable = true;
                equipment.Condition = request.Condition; // Update condition based on return form
                _unitOfWork.Equipment.UpdateEquipment(equipment);
            }

            // Save changes to database
            _unitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Rental>> GetActiveRentalsAsync()
        {
            // Get all active rentals from repository
            var activeRentals = _unitOfWork.Rentals.GetActiveRentals();

            return Task.FromResult(activeRentals); // Wrap in Task
        }

        public Task<IEnumerable<Rental>> GetCompletedRentalsAsync()
        {
            // Get all rentals where ReturnedAt is not null
            var completedRentals = _unitOfWork.Rentals.GetCompletedRentals();

            return Task.FromResult(completedRentals);
        }

        public Task<IEnumerable<Rental>> GetOverdueRentalsAsync()
        {
            // Get all rentals where DueDate < Now and ReturnedAt is null
            var overdueRentals = _unitOfWork.Rentals.GetOverdueRentals();


            return Task.FromResult(overdueRentals);
        }

        public Task<IEnumerable<Rental>> GetRentalHistoryByEquipmentAsync(int equipmentId)
        {
            // Get all rentals for the specified equipment
            var rentalHistory = _unitOfWork.Rentals.GetEquipmentRentalHistory(equipmentId);
            
            return Task.FromResult(rentalHistory);
        }

        public Task ExtendRentalAsync(int rentalId, ExtensionRequest request)
        {
            // Get rental by id
            var rental = _unitOfWork.Rentals.GetRentalDetails(rentalId);
            if (rental == null)
            {
                throw new KeyNotFoundException($"Rental with ID {rentalId} not found.");
            }

            if (rental.ReturnedAt.HasValue)
            {
                throw new InvalidOperationException("Cannot extend a completed rental.");
            }

            // Update due date and extension reason
            rental.DueDate = request.NewDueDate;
            rental.ExtensionReason = request.Reason;

            _unitOfWork.Rentals.UpdateRental(rental);

            // Save changes to database
            _unitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task CancelRentalAsync(int rentalId)
        {
            // Get rental by id
            var rental = _unitOfWork.Rentals.GetRentalDetails(rentalId);
            if (rental == null)
            {
                throw new KeyNotFoundException($"Rental with ID {rentalId} not found.");
            }

            // Force return: set ReturnedAt to now
            rental.ReturnedAt = DateTime.Now;

            // Calculate overdue fee if applicable
            if (rental.ReturnedAt > rental.DueDate)
            {
                int overdueDays = (rental.ReturnedAt.Value - rental.DueDate).Days;
                rental.OverdueFee = overdueDays * OverdueFeePerDay;
            }

            _unitOfWork.Rentals.UpdateRental(rental);

            // Update equipment availability
            var equipment = _unitOfWork.Equipment.GetSpecificEquipment(rental.EquipmentId);
            if (equipment != null)
            {
                equipment.IsAvailable = true;
                _unitOfWork.Equipment.UpdateEquipment(equipment);
            }

            // Save changes to database
            _unitOfWork.SaveChanges();

            return Task.CompletedTask;
        }
    }
}