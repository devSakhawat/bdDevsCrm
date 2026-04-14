using bdDevCRM.ServiceContract.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Presentation.Api.BackgroundServices;

public class TokenCleanupBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TokenCleanupBackgroundService> _logger;
    private readonly IConfiguration _configuration;
    private TimeSpan _cleanupInterval;
    private TimeSpan _retryDelay;

    public TokenCleanupBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<TokenCleanupBackgroundService> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
        
        // Read configuration values with defaults
        var intervalHours = _configuration.GetValue<int?>("TokenCleanup:IntervalHours") ?? 24;
        var retryMinutes = _configuration.GetValue<int?>("TokenCleanup:RetryDelayMinutes") ?? 5;
        
        _cleanupInterval = TimeSpan.FromHours(intervalHours);
        _retryDelay = TimeSpan.FromMinutes(retryMinutes);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Token Cleanup Background Service started (Interval: {Interval}, Retry Delay: {RetryDelay})", 
            _cleanupInterval, _retryDelay);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PerformCleanupAsync(stoppingToken);
                await Task.Delay(_cleanupInterval, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during token cleanup");
                await Task.Delay(_retryDelay, stoppingToken);
            }
        }
    }

    private async Task PerformCleanupAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();

        _logger.LogInformation("Starting expired token cleanup at {Time}", DateTime.UtcNow);

        await authService.RemoveExpiredTokensAsync();

        _logger.LogInformation("Token cleanup completed at {Time}", DateTime.UtcNow);
    }
}
