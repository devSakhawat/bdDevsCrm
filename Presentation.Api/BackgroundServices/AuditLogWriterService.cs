using Domain.Entities.Entities.System;
using Infrastructure.Sql.Context;
using Presentation.Api.BackgroundServices;

namespace Presentation.Api.BackgroundServices;

/// <summary>
/// Background service — queue থেকে audit logs নিয়ে batch insert করে।
/// 
/// কেন BackgroundService?
/// - Request thread block হয় না
/// - Batch insert = কম DB round trips
/// - App shutdown হলে gracefully বাকি logs flush করে
/// </summary>
public class AuditLogWriterService : BackgroundService
{
  private readonly AuditLogQueue _queue;
  private readonly IServiceScopeFactory _scopeFactory;
  private readonly ILogger<AuditLogWriterService> _logger;
  private readonly int _batchSize;
  private readonly TimeSpan _flushInterval;

  public AuditLogWriterService(
      AuditLogQueue queue,
      IServiceScopeFactory scopeFactory,
      ILogger<AuditLogWriterService> logger,
      IConfiguration configuration)
  {
    _queue = queue;
    _scopeFactory = scopeFactory;
    _logger = logger;
    _batchSize = configuration.GetValue("AuditLogging:BatchSize", 100);
    _flushInterval = TimeSpan.FromSeconds(
        configuration.GetValue("AuditLogging:FlushIntervalSeconds", 5));
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    _logger.LogInformation("Audit log writer started. BatchSize: {BatchSize}, FlushInterval: {Interval}s",
        _batchSize, _flushInterval.TotalSeconds);

    var batch = new List<AuditLog>(_batchSize);

    try
    {
      await foreach (var log in _queue.DequeueAllAsync(stoppingToken))
      {
        batch.Add(log);

        // Batch full হলে flush করো
        if (batch.Count >= _batchSize)
        {
          await FlushBatchAsync(batch, stoppingToken);
          batch.Clear();
        }
      }
    }
    catch (OperationCanceledException)
    {
      // App shutting down — বাকি logs flush করো
      _logger.LogInformation("Audit log writer stopping. Flushing {Count} remaining logs...", batch.Count);
    }
    finally
    {
      // Graceful shutdown: বাকি logs save করো
      if (batch.Count > 0)
      {
        await FlushBatchAsync(batch, CancellationToken.None);
      }

      // Queue তে আরো remaining থাকতে পারে
      await DrainRemainingAsync();
    }
  }

  private async Task FlushBatchAsync(List<AuditLog> batch, CancellationToken ct)
  {
    if (batch.Count == 0) return;

    try
    {
      using var scope = _scopeFactory.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<CRMContext>();

      dbContext.AuditLogs.AddRange(batch);
      await dbContext.SaveChangesAsync(ct);

      _logger.LogDebug("Flushed {Count} audit logs to database", batch.Count);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to flush {Count} audit logs to database", batch.Count);
      // Logs হারিয়ে যাবে — কিন্তু app crash হবে না
      // Production এ dead-letter queue বা file fallback যোগ করতে পারেন
    }
  }

  private async Task DrainRemainingAsync()
  {
    var remaining = new List<AuditLog>();
    while (_queue.Count > 0)
    {
      // Try to read without waiting
      await foreach (var log in _queue.DequeueAllAsync(new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token))
      {
        remaining.Add(log);
        if (remaining.Count >= _batchSize)
        {
          await FlushBatchAsync(remaining, CancellationToken.None);
          remaining.Clear();
        }
      }
    }

    if (remaining.Count > 0)
    {
      await FlushBatchAsync(remaining, CancellationToken.None);
    }
  }
}