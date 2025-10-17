using Midterm_EquipmentRental_Team5.Models.Interfaces;

namespace Midterm_EquipmentRental_Team5.Models.DTOs
{
    public class LoginResponse : ILoginResponse
    {
        public string Token { get; set; }
    }
}
