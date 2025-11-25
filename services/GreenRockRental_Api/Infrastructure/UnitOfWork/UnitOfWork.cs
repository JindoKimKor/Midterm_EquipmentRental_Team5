using GreenRockRental_Api.Infrastructure.Persistence;
using GreenRockRental_Api.Infrastructure.Repositories.Interfaces;
using GreenRockRental_Api.Infrastructure.UnitOfWork;

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

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
