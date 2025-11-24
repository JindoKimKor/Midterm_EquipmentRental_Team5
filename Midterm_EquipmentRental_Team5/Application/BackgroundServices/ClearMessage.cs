using Microsoft.AspNetCore.SignalR;
using Midterm_EquipmentRental_Team5.Application.Services.Interfaces;
using Midterm_EquipmentRental_Team5.Presentation.Hubs;

namespace Midterm_EquipmentRental_Team5.Application.BackgroundServices
{
    public class ClearMessage(IHubContext<ChatHub> hubContext, IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly IHubContext<ChatHub> _hubContext = hubContext;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Create scope to access scoped IChatService
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var chatService = scope.ServiceProvider.GetRequiredService<IChatService>();
                        chatService.ClearMessageFromDb();
                    }

                    await _hubContext.Clients.All.SendAsync("ClearAllMessages", cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in ClearMessage background service: {ex.Message}");
                }

                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}