namespace GreenRockRental_Api.Application.DTOs
{
    /// <summary>
    /// DTO for chat details - complete chat conversation
    /// </summary>
    public class ChatDetailDto
    {
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public string? SenderName { get; set; }
        public int ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public List<MessageDto> Messages { get; set; } = new List<MessageDto>();
    }
}
