
using MobileFinance.Infra.Services.Seeding;

namespace MobileFinance.API.BackgroundServices;

public class CalendarSeederService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public CalendarSeederService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var calendarSeeder = scope.ServiceProvider.GetRequiredService<CalendarSeeder>();

            await calendarSeeder.Seed();

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
