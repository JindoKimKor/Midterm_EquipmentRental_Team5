using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;
using Midterm_EquipmentRental_Team5.UnitOfWork.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IUserRespository Users { get; set; }
    public IEquipmentRepository Equipments { get; }
    public ICustomerRepository Customers { get; }
    public IRentalRepository Rentals { get; }

    public UnitOfWork(AppDbContext context,
                      IEquipmentRepository equipmentRepository,
                      ICustomerRepository customerRepository,
                      IRentalRepository rentalRepository,
                      IUserRespository user)
    {
        _context = context;
        Equipments = equipmentRepository;
        Customers = customerRepository;
        Rentals = rentalRepository;
        Users = user;
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
}
