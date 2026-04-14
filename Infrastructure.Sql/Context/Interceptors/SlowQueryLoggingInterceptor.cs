using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace Infrastructure.Sql.Context.Interceptors;

public class SlowQueryLoggingInterceptor : DbCommandInterceptor
{
    private readonly ILogger<SlowQueryLoggingInterceptor> _logger;
    private readonly IConfiguration _configuration;
    private readonly int _slowQueryThresholdMs;
    private readonly int _verySlowQueryThresholdMs;

    public SlowQueryLoggingInterceptor(ILogger<SlowQueryLoggingInterceptor> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _slowQueryThresholdMs = configuration.GetValue<int>("PerformanceMonitoring:SlowQueryThresholdMs", 500);
        _verySlowQueryThresholdMs = configuration.GetValue<int>("PerformanceMonitoring:VerySlowQueryThresholdMs", 2000);
    }

    public override ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = default)
    {
        var elapsedMs = eventData.Duration.TotalMilliseconds;

        if (elapsedMs >= _verySlowQueryThresholdMs)
        {
            _logger.LogWarning(
                "VERY SLOW QUERY detected: {Query} took {Duration}ms",
                SanitizeQuery(command.CommandText),
                elapsedMs);
        }
        else if (elapsedMs >= _slowQueryThresholdMs)
        {
            _logger.LogWarning(
                "SLOW QUERY detected: {Query} took {Duration}ms",
                SanitizeQuery(command.CommandText),
                elapsedMs);
        }

        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override ValueTask<int> NonQueryExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        var elapsedMs = eventData.Duration.TotalMilliseconds;

        if (elapsedMs >= _verySlowQueryThresholdMs)
        {
            _logger.LogWarning(
                "VERY SLOW NON-QUERY: {Query} took {Duration}ms",
                SanitizeQuery(command.CommandText),
                elapsedMs);
        }

        return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
    }

    private string SanitizeQuery(string query)
    {
        const int maxLength = 500;
        if (query.Length > maxLength)
        {
            return query.Substring(0, maxLength) + "...";
        }
        return query;
    }
}
