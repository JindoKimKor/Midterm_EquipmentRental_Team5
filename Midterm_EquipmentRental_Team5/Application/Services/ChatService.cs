using Microsoft.AspNetCore.SignalR;
using Midterm_EquipmentRental_Team5.Presentation.Hubs;

namespace Midterm_EquipmentRental_Team5.Application.Services
{
    public class ChatServices(IHubContext<ChatHub> chatHubContext)
    {
        private readonly IHubContext<ChatHub> _chatHubContext = chatHubContext;
    }
}