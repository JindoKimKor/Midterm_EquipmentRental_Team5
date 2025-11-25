using System.ComponentModel.DataAnnotations;
using GreenRockRental_Api.Domain.Interfaces;

namespace GreenRockRental_Api.Domain.Entities
{
    public class Customer : ICustomer
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? State { get; set; }
        public string? Role { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ExternalProvider { get; set; } // OAuth provider (e.g., "Google")
        public string? ExternalId { get; set; } // External user ID from OAuth provider
    }
}
