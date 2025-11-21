using Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IEquipmentRepository Equipments { get; }
        ICustomerRepository Customers { get; }
        IRentalRepository Rentals { get; }
        int SaveChanges();
    }
}
