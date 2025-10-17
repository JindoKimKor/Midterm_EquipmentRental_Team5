using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IEquipmentRepository Equipments { get; }
    public ICustomerRepository Customers { get; }
    public IRentalRepository Rentals { get; }

    public UnitOfWork(AppDbContext context,
                      IEquipmentRepository equipmentRepository,
                      ICustomerRepository customerRepository,
                      IRentalRepository rentalRepository)
    {
        _context = context;
        Equipments = equipmentRepository;
        Customers = customerRepository;
        Rentals = rentalRepository;
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
}
