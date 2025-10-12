namespace Midterm_EquipmentRental_Team5.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int CustomerId { get; set; }
        public DateTime IssuedAt { get; set; }

        // ReturnedAt can be empty until customer returns their equipment
        public DateTime? ReturnedAt { get; set; }
    }
}
