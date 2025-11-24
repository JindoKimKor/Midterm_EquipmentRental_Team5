using Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.Application.BackgroundServices
{
    public class ClearMessage(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                    chatRepository.ClearAllMessages();
                }
                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}