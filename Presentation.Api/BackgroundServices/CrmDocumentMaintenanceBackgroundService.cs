using Domain.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Presentation.Api.BackgroundServices;

public class CrmDocumentMaintenanceBackgroundService : BackgroundService
{
    private const string IntervalHoursConfigKey = "CrmDocumentJobs:IntervalHours";
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CrmDocumentMaintenanceBackgroundService> _logger;
    private readonly IConfiguration _configuration;

    public CrmDocumentMaintenanceBackgroundService(IServiceProvider serviceProvider, ILogger<CrmDocumentMaintenanceBackgroundService> logger, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalHours = _configuration.GetValue<int?>(IntervalHoursConfigKey) ?? 24;
        var interval = TimeSpan.FromHours(intervalHours);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var serviceManager = scope.ServiceProvider.GetRequiredService<IServiceManager>();
                var escalated = await serviceManager.CrmStudentDocuments.EscalateRejectedDocumentsAsync(stoppingToken);
                _logger.LogInformation("CRM document maintenance completed. Escalated={Escalated}", escalated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CRM document maintenance job failed.");
            }
            await Task.Delay(interval, stoppingToken);
        }
    }
}
