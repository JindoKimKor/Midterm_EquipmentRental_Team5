using Midterm_EquipmentRental_Team5.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IEquipmentRepository Equipment { get; }
        ICustomerRepository Customers { get; }
        IRentalRepository Rentals { get; }

        int SaveChanges();
    }
}
