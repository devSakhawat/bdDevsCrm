using Domain.Entities.Entities.System;
using System.Threading.Channels;

namespace Presentation.Api.BackgroundServices;

/// <summary>
/// Bounded queue — audit log async write করার জন্য।
/// Task.Run এর বদলে এটা ব্যবহার করুন।
/// 
/// কেন Channel<T>?
/// - Thread-safe
/// - Bounded (memory overflow হবে না)
/// - Backpressure support (queue full হলে পুরানো log drop করে)
/// - Zero allocation after warmup
/// </summary>
public class AuditLogQueue
{
  private readonly Channel<AuditLog> _channel;
  private readonly ILogger<AuditLogQueue> _logger;

  public AuditLogQueue(ILogger<AuditLogQueue> logger, IConfiguration configuration)
  {
    _logger = logger;

    var capacity = configuration.GetValue("AuditLogging:QueueCapacity", 10_000);

    _channel = Channel.CreateBounded<AuditLog>(new BoundedChannelOptions(capacity)
    {
      // Queue full হলে সবচেয়ে পুরানো item drop করবে
      // request কখনো block হবে না
      FullMode = BoundedChannelFullMode.DropOldest,
      SingleReader = false,  // multiple consumers possible
      SingleWriter = false   // multiple middleware threads write করবে
    });
  }

  /// <summary>
  /// Audit log queue তে রাখো — non-blocking, thread-safe
  /// </summary>
  public bool TryEnqueue(AuditLog log)
  {
    if (_channel.Writer.TryWrite(log))
    {
      return true;
    }

    _logger.LogWarning("Audit log queue is full. Dropping oldest log.");
    return false;
  }

  /// <summary>
  /// Background service এটা ব্যবহার করে queue থেকে পড়বে
  /// </summary>
  public IAsyncEnumerable<AuditLog> DequeueAllAsync(CancellationToken ct)
      => _channel.Reader.ReadAllAsync(ct);

  /// <summary>
  /// Queue তে কতগুলো item আছে (monitoring এর জন্য)
  /// </summary>
  public int Count => _channel.Reader.Count;
}