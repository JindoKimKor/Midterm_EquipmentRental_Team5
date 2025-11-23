namespace Midterm_EquipmentRental_Team5.Application.DTOs
{
    public class ChatRequest
    {
        public int ChatId { get; set; }
        public string ChatName { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}