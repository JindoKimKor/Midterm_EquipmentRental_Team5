namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    /// <summary>
    /// DTO for message details
    /// </summary>
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string? SenderName { get; set; }
        public int ReceiverId { get; set; }
        public string? Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public int ChatId { get; set; }
    }
}
