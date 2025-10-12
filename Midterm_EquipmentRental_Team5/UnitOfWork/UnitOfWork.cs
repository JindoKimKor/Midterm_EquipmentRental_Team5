using Midterm_EquipmentRental_Team5.Repositories.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

namespace Midterm_EquipmentRental_Team5.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IEquipmentRepository Equipment => throw new NotImplementedException();

        public ICustomerRepository Customers => throw new NotImplementedException();

        public IRentalRepository Rentals => throw new NotImplementedException();

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
