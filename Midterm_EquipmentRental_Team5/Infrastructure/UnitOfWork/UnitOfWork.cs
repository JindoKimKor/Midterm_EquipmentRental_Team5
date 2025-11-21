using Midterm_EquipmentRental_Team5.Infrastructure.Persistence;
using Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces;
using Midterm_EquipmentRental_Team5.Infrastructure.UnitOfWork;

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
