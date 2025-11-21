namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    public class MessageDto
    {
        public Guid MessageId { get; set; }
        public Guid ChatId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}