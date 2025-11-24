namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    /// <summary>
    /// DTO for rental list - summary view of rentals
    /// </summary>
    public class RentalListDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int EquipmentId { get; set; }
        public string? EquipmentName { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public bool IsActive { get; set; }
        public decimal? OverdueFee { get; set; }
    }
}
