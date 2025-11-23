using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team5.Domain.Entities;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    Email = "chrisy.dinh03@gmail.com",
                    Role = "Admin",
                    ExternalProvider = "Google",
                    ExternalId = null
                },
                new AppUser
                {
                    Id = 2,
                    Email = "n.dinh72943@gmail.com,",
                    Role = "User",
                    ExternalProvider = "Google",
                    ExternalId = null
                },
                new AppUser
                {
                    Id = 3,
                    Email = "user2@example.com",
                    Role = "User",
                    ExternalProvider = "Google",
                    ExternalId = null
                }
            );

            // Seed Customers (1 Admin + 5 Users)
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

            // Seed Equipment (5+ items - different categories)
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
                    CreatedAt = new DateTime(2024, 1, 1),
                    ImageUrl = "/images/equipment/Excavator.png"
                },
                new Equipment
                {
                    Id = 2,
                    Name = "Concrete Mixer",
                    Description = "Portable concrete mixer for medium-sized jobs",
                    Category = "Heavy Machinery",
                    Condition = "Good",
                    RentalPrice = 150.00m,
                    IsAvailable = false,
                    CreatedAt = new DateTime(2024, 1, 1),
                    ImageUrl = "/images/equipment/Concrete.png"
                },
                new Equipment
                {
                    Id = 3,
                    Name = "Cordless Drill Set",
                    Description = "Professional grade cordless drill with accessories",
                    Category = "Power Tools",
                    Condition = "New",
                    RentalPrice = 30.00m,
                    IsAvailable = false,
                    CreatedAt = new DateTime(2024, 1, 1),
                    ImageUrl = "/images/equipment/CordlessDrill.png"
                },
                new Equipment
                {
                    Id = 4,
                    Name = "Pickup Truck F-150",
                    Description = "Heavy duty pickup truck for material transport",
                    Category = "Vehicles",
                    Condition = "Good",
                    RentalPrice = 200.00m,
                    IsAvailable = true,
                    CreatedAt = new DateTime(2024, 1, 1),
                    ImageUrl = "/images/equipment/Truck.png"
                },
                new Equipment
                {
                    Id = 5,
                    Name = "Safety Harness Kit",
                    Description = "Complete safety harness kit for elevated work",
                    Category = "Safety",
                    Condition = "Excellent",
                    RentalPrice = 25.00m,
                    IsAvailable = true,
                    CreatedAt = new DateTime(2024, 1, 1),
                    ImageUrl = "/images/equipment/Hardness_Kit.png"
                },
                new Equipment
                {
                    Id = 6,
                    Name = "Laser Level",
                    Description = "Professional laser level for precise measurements",
                    Category = "Surveying",
                    Condition = "New",
                    RentalPrice = 40.00m,
                    IsAvailable = true,
                    CreatedAt = new DateTime(2024, 1, 1),
                    ImageUrl = "/images/equipment/Laser_Level.png"
                }
            );

            modelBuilder.Entity<Rental>().HasData(
                // Active rental - John has the drill
                new Rental
                {
                    Id = 1,
                    EquipmentId = 3, // Cordless Drill
                    CustomerId = 2, // John Doe
                    IssuedAt = DateTime.Now.AddDays(-3),
                    DueDate = DateTime.Now.AddDays(4),
                    ReturnedAt = null,
                    IsActive = true, // Active rental
                    OverdueFee = null,
                    ExtensionReason = null
                },
                // Completed rental - Jane returned the excavator
                new Rental
                {
                    Id = 2,
                    EquipmentId = 1, // Excavator
                    CustomerId = 3, // Jane Smith
                    IssuedAt = DateTime.Now.AddDays(-14),
                    DueDate = DateTime.Now.AddDays(-7),
                    ReturnedAt = DateTime.Now.AddDays(-6),
                    IsActive = false, // Completed
                    OverdueFee = null,
                    ExtensionReason = null
                },
                // Overdue rental - Bob has the concrete mixer and it's overdue
                new Rental
                {
                    Id = 3,
                    EquipmentId = 2, // Concrete Mixer
                    CustomerId = 4, // Bob Johnson
                    IssuedAt = DateTime.Now.AddDays(-10),
                    DueDate = DateTime.Now.AddDays(-2), // Overdue by 2 days
                    ReturnedAt = null,
                    IsActive = true, // Active but overdue
                    OverdueFee = null, // Will be calculated when returned
                    ExtensionReason = null
                }
            );

            modelBuilder.Entity<Chat>().HasData(
                new Chat { ChatId = 1, SenderId = 2, ReceiverId = 1 },
                new Chat { ChatId = 2, SenderId = 3, ReceiverId = 1 },
                new Chat { ChatId = 3, SenderId = 4, ReceiverId = 1 },
                new Chat { ChatId = 4, SenderId = 5, ReceiverId = 1 },
                new Chat { ChatId = 5, SenderId = 6, ReceiverId = 1 }
            );

            modelBuilder.Entity<Message>().HasData(
                new Message
                {
                    Id = 1,
                    SenderId = "2",
                    ReceiverId = "1",
                    Content = "Hi Admin, I need help with a rental.",
                    Timestamp = DateTime.UtcNow,
                    ChatId = 1
                },
                new Message
                {
                    Id = 2,
                    SenderId = "1",
                    ReceiverId = "2",
                    Content = "Hello John, how can I assist you?",
                    Timestamp = DateTime.UtcNow,
                    ChatId = 1
                },

                new Message
                {
                    Id = 3,
                    SenderId = "3",
                    ReceiverId = "1",
                    Content = "Hello Admin, I have a billing question.",
                    Timestamp = DateTime.UtcNow,
                    ChatId = 2
                },
                new Message
                {
                    Id = 4,
                    SenderId = "1",
                    ReceiverId = "3",
                    Content = "Hi Jane, sure — what's your question?",
                    Timestamp = DateTime.UtcNow,
                    ChatId = 2
                },

                new Message
                {
                    Id = 5,
                    SenderId = "4",
                    ReceiverId = "1",
                    Content = "Hi Admin, is the lawn mower available?",
                    Timestamp = DateTime.UtcNow,
                    ChatId = 3
                },
                new Message
                {
                    Id = 6,
                    SenderId = "1",
                    ReceiverId = "4",
                    Content = "Yes Bob, it's available for rental.",
                    Timestamp = DateTime.UtcNow,
                    ChatId = 3
                },

                new Message
                {
                    Id = 7,
                    SenderId = "5",
                    ReceiverId = "1",
                    Content = "Hello Admin, I want to extend my rental.",
                    Timestamp = DateTime.UtcNow,
                    ChatId = 4
                },
                new Message
                {
                    Id = 8,
                    SenderId = "1",
                    ReceiverId = "5",
                    Content = "Hi Alice, I can process that for you.",
                    Timestamp = DateTime.UtcNow,
                    ChatId = 4
                },

                new Message
                {
                    Id = 9,
                    SenderId = "6",
                    ReceiverId = "1",
                    Content = "Admin, I can't log in to my account.",
                    Timestamp = DateTime.UtcNow,
                    ChatId = 5
                },
                new Message
                {
                    Id = 10,
                    SenderId = "1",
                    ReceiverId = "6",
                    Content = "Hey Charlie, let me help you reset your password.",
                    Timestamp = DateTime.UtcNow,
                    ChatId = 5
                }
            );
        }
    }
}