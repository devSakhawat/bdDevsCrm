using Domain.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Presentation.Api.BackgroundServices;

public class CrmFollowUpMaintenanceBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CrmFollowUpMaintenanceBackgroundService> _logger;
    private readonly IConfiguration _configuration;

    public CrmFollowUpMaintenanceBackgroundService(IServiceProvider serviceProvider, ILogger<CrmFollowUpMaintenanceBackgroundService> logger, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalHours = _configuration.GetValue<int?>("CrmFollowUpJobs:IntervalHours") ?? 24;
        var interval = TimeSpan.FromHours(intervalHours);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var serviceManager = scope.ServiceProvider.GetRequiredService<IServiceManager>();

                var overdue = await serviceManager.CrmFollowUps.ProcessOverdueFollowUpsAsync(stoppingToken);
                var unresponsive = await serviceManager.CrmFollowUps.ProcessUnresponsiveLeadsAsync(stoppingToken);
                _logger.LogInformation("CRM follow-up maintenance completed. Overdue={Overdue}, Unresponsive={Unresponsive}", overdue, unresponsive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CRM follow-up maintenance job failed.");
            }

            await Task.Delay(interval, stoppingToken);
        }
    }
}
