using Midterm_EquipmentRental_Team5.Domain.Interfaces;

namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    public class LoginResponse : ILoginResponse
    {
        public string Token { get; set; }
    }
}
