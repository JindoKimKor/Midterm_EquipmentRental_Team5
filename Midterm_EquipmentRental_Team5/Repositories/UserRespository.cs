using Midterm_EquipmentRental_Team5.Data;
using Midterm_EquipmentRental_Team5.Models.Interfaces;
using Midterm_EquipmentRental_Team5.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.Repositories
{
    public class UserReposityory : IUserRespository
    {
        private readonly AppDbContext _context;

        public UserReposityory(AppDbContext context)
        {
            _context = context;
        }

        public IUser GetUserByPasswordAndUserName(string password, string userName)
        {
            return _context.Users.First(u => u.HashedPassword == password && u.UserName == userName);
        }
    }
}
