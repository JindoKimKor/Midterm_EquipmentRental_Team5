namespace Midterm_EquipmentRental_Team5.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int CustomerId { get; set; }
        public DateTime IssuedAt { get; set; }

        // New Property regarding Overdue Calculation: Overdue = DueDate < DateTime.Now && ReturnedAt == null
        public DateTime DueDate { get; set; }
        // ReturnedAt can be empty until customer returns their equipment
        public DateTime? ReturnedAt { get; set; }

    }
}
