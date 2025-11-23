using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team5.Infrastructure.Persistence;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly AppDbContext _context;

        public AppUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public AppUser? GetByEmailAsync(string email)
        {
            return _context.Set<AppUser>()
                .FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
