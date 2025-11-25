using GreenRockRental_Api.Infrastructure.Repositories.Interfaces;

namespace GreenRockRental_Api.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IEquipmentRepository Equipments { get; }
        ICustomerRepository Customers { get; }
        IRentalRepository Rentals { get; }
        Task<int> SaveChangesAsync();
    }
}
