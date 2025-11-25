namespace GreenRockRental_Api.Application.DTOs
{
    /// <summary>
    /// DTO for equipment details - complete equipment information
    /// </summary>
    public class EquipmentDetailDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Condition { get; set; }
        public decimal RentalPrice { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
