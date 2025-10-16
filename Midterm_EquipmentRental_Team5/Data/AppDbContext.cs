using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team5.Models;
using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<IUser> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    Name = "Admin User",
                    Email = "admin@rental.com",
                    UserName = "admin",
                    Password = "admin123",
                    Role = "Admin"
                },
                new Customer { Id = 2, Name = "John Doe", Email = "john@example.com", UserName = "user1", Password = "user1", Role = "User" },
                new Customer { Id = 3, Name = "Jane Smith", Email = "jane@example.com", UserName = "user2", Password = "user2", Role = "User" },
                new Customer { Id = 4, Name = "Bob Johnson", Email = "bob@example.com", UserName = "user3", Password = "user3", Role = "User" },
                new Customer { Id = 5, Name = "Alice Williams", Email = "alice@example.com", UserName = "user4", Password = "user4", Role = "User" },
                new Customer { Id = 6, Name = "Charlie Brown", Email = "charlie@example.com", UserName = "user5", Password = "user5", Role = "User" }
            );

            // Seed Equipment (5 items - different categories)
            // Equipment Categories: Heavy Machinery, Power Tools, Vehicles, Safety, Surveying
            // Equipment Conditions: New, Excellent, Good, Fair, Poor
            modelBuilder.Entity<Equipment>().HasData(
                new Equipment
                {
                    Id = 1,
                    Name = "Excavator CAT 320",
                    Description = "Heavy duty excavator for large construction projects",
                    Category = "Heavy Machinery",
                    Condition = "Excellent",
                    RentalPrice = 500.00m,
                    IsAvailable = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Equipment { Id = 2, Name = "Concrete Mixer", Description = "Portable concrete mixer for medium-sized jobs", Category = "Heavy Machinery", Condition = "Good", RentalPrice = 150.00m, IsAvailable = false, CreatedAt = new DateTime(2024, 1, 1) },
                new Equipment { Id = 3, Name = "Cordless Drill Set", Description = "Professional grade cordless drill with accessories", Category = "Power Tools", Condition = "New", RentalPrice = 30.00m, IsAvailable = false, CreatedAt = new DateTime(2024, 1, 1) },
                new Equipment { Id = 4, Name = "Pickup Truck F-150", Description = "Heavy duty pickup truck for material transport", Category = "Vehicles", Condition = "Good", RentalPrice = 200.00m, IsAvailable = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Equipment { Id = 5, Name = "Safety Harness Kit", Description = "Complete safety harness kit for elevated work", Category = "Safety", Condition = "Excellent", RentalPrice = 25.00m, IsAvailable = true, CreatedAt = new DateTime(2024, 1, 1) }
            );

            // Seed Rentals (Active, Completed, Overdue)
            modelBuilder.Entity<Rental>().HasData(
                new Rental
                {
                    Id = 1,
                    EquipmentId = 3,
                    CustomerId = 2,
                    IssuedAt = DateTime.Now.AddDays(-3),
                    DueDate = DateTime.Now.AddDays(4),
                    ReturnedAt = null
                },  // Active
                new Rental { Id = 2, EquipmentId = 1, CustomerId = 3, IssuedAt = DateTime.Now.AddDays(-14), DueDate = DateTime.Now.AddDays(-7), ReturnedAt = DateTime.Now.AddDays(-6) },  // Completed
                new Rental { Id = 3, EquipmentId = 2, CustomerId = 4, IssuedAt = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(-2), ReturnedAt = null }  // Overdue
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "admin",
                    HashedPassword = "1",
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    UserName = "user",
                    HashedPassword = "2",
                    Role = "User"
                }
            );
        }
    }
}
