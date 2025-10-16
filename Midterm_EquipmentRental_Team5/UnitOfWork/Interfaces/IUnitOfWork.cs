using Midterm_EquipmentRental_Team5.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IEquipmentRepository Equipments { get; }
        ICustomerRepository Customers { get; }
        IRentalRepository Rentals { get; }
        IUserRespository Users { get; }
        int SaveChanges();
    }
}
