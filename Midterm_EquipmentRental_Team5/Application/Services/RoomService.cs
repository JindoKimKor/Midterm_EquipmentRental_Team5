using Microsoft.AspNetCore.SignalR;
using Midterm_EquipmentRental_Team5.Presentation.Hubs;

namespace Midterm_EquipmentRental_Team5.Application.Services
{
    public class RoomService(IHubContext<RoomHub> roomHubContext)
    {
        public readonly IHubContext<RoomHub> _roomHubContext = roomHubContext;
    }
}