namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    /// <summary>
    /// DTO for rental details - complete rental information
    /// </summary>
    public class RentalDetailDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public int EquipmentId { get; set; }
        public string? EquipmentName { get; set; }
        public string? EquipmentCategory { get; set; }
        public decimal RentalPrice { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public bool IsActive { get; set; }
        public decimal? OverdueFee { get; set; }
        public string? ExtensionReason { get; set; }
    }
}
