using Presentation.Api.Middleware.Shared;
using Domain.Entities.Entities.System;
using Presentation.Api.BackgroundServices;
using System.Security.Claims;

namespace Presentation.Api.Middleware;

/// <summary>
/// Enhanced audit logging middleware.
/// 
/// Conflict #2 resolved: Uses PipelineContext.Stopwatch (shared timer)
/// Conflict #3 resolved: Uses RequestBodyReader.ReadOnceAsync() (cached body)
/// Conflict #4 resolved: Uses PipelineContext.CorrelationId (shared ID)
/// Task.Run removed: Uses AuditLogQueue + BackgroundService (batched DB write)
/// Response body buffering removed (unnecessary in production)
/// </summary>
public class EnhancedAuditMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<EnhancedAuditMiddleware> _logger;
  private readonly IConfiguration _configuration;
  private readonly AuditLogQueue _auditQueue;
  private readonly int _maxCaptureBytes;

  public EnhancedAuditMiddleware(RequestDelegate next, ILogger<EnhancedAuditMiddleware> logger, IConfiguration configuration, AuditLogQueue auditQueue)          // inject here
  {
    _next = next;
    _logger = logger;
    _configuration = configuration;
    _auditQueue = auditQueue;          // assign here
    _maxCaptureBytes = _configuration.GetValue("AuditLogging:MaxBodyCaptureBytes", 4096);
  }

  public async Task InvokeAsync(HttpContext context)
  {
    // Check if audit is enabled
    if (!_configuration.GetValue<bool>("AuditLogging:EnableAuditMiddleware", true))
    {
      await _next(context);
      return;
    }

    // Skip certain paths (health, swagger, uploads, etc.)
    if (ShouldSkipAudit(context.Request.Path))
    {
      await _next(context);
      return;
    }

    //  shared context (stopwatch, correlationId already set by earlier middleware)
    var pipelineCtx = RequestPipelineContext.OrCreate(context);

    try
    {
      // Request body — uses cached version (will NOT read again)
      // RequestBodyReader.ReadOnceAsync checks if body was already read
      // If StructuredLoggingMiddleware already read it, this returns cached copy instantly
      string? requestBody = null;
      if (context.Request.Method != "GET")
      {
        requestBody = await RequestBodyReader.ReadOnceAsync(context, _maxCaptureBytes);
      }

      // Execute next middleware / controller
      await _next(context);

      // Read duration from shared stopwatch (not a new one)
      var durationMs = pipelineCtx.Stopwatch.ElapsedMilliseconds;

      // Create audit log entry
      var auditLog = CreateAuditLog(context, requestBody, durationMs, pipelineCtx.CorrelationId);

      // Enqueue instead of Task.Run
      // Non-blocking, thread-safe, bounded queue
      // AuditLogWriterService will batch-insert to DB in background
      if (!_auditQueue.TryEnqueue(auditLog))
      {
        _logger.LogWarning(
            "Audit log queue full. Dropping audit for {Method} {Path} | CorrelationId: {CorrelationId}",
            context.Request.Method, context.Request.Path, pipelineCtx.CorrelationId);
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error in audit middleware | CorrelationId: {CorrelationId}",
          pipelineCtx.CorrelationId);

      // Still try to log the error in audit
      var errorAuditLog = CreateErrorAuditLog(
          context,
          ex,
          pipelineCtx.Stopwatch.ElapsedMilliseconds,
          pipelineCtx.CorrelationId);

      _auditQueue.TryEnqueue(errorAuditLog);

      throw; // Re-throw so StandardExceptionMiddleware handles it
    }
  }

  #region Path Filtering
  private bool ShouldSkipAudit(PathString path)
  {
    var pathValue = path.Value?.ToLower() ?? string.Empty;
    var skipPaths = _configuration.GetSection("AuditLogging:SkipPaths").Get<string[]>() ?? Array.Empty<string>();

    foreach (var skipPath in skipPaths)
    {
      if (pathValue.Contains(skipPath.ToLower())) return true;
    }

    return false;
  }
  #endregion

  #region Audit Log Creation
  private AuditLog CreateAuditLog(HttpContext context, string? requestBody, long durationMs, string correlationId)
  {
    var user = context.User;
    var request = context.Request;
    var response = context.Response;

    // Safe session access: check feature first to avoid exception
    string? sessionId = null;
    var sessionFeature = context.Features.Get<Microsoft.AspNetCore.Http.Features.ISessionFeature>();
    if (sessionFeature != null)
    {
      try { sessionId = context.Session?.Id; }
      catch { sessionId = null; }
    }

    return new AuditLog
    {
      // User info
      UserId = UserId(user),
      Username = user?.Identity?.Name ?? "Anonymous",
      IpAddress = context.Connection.RemoteIpAddress?.ToString(),
      UserAgent = request.Headers["User-Agent"].FirstOrDefault(),

      // Action info
      Action = ActionFromMethod(request.Method),
      EntityType = EntityTypeFromPath(request.Path),
      EntityId = EntityIdFromPath(request.Path),
      Endpoint = $"{request.Method} {request.Path}",
      Module = ModuleFromPath(request.Path),

      // Data
      NewValue = requestBody,

      // Timestamps
      Timestamp = DateTime.UtcNow,

      // Tracking
      CorrelationId = correlationId,
      SessionId = sessionId,
      RequestId = context.TraceIdentifier,

      // Result
      Success = response.StatusCode >= 200 && response.StatusCode < 300,
      StatusCode = response.StatusCode,
      DurationMs = (int)durationMs
    };
  }

  private AuditLog CreateErrorAuditLog(HttpContext context, Exception ex, long durationMs, string correlationId)
  {
    var auditLog = CreateAuditLog(context, null, durationMs, correlationId);
    auditLog.Success = false;
    auditLog.StatusCode = 500;
    auditLog.ErrorMessage = $"{ex.GetType().Name}: {ex.Message}";
    return auditLog;
  }

  #endregion

  #region Helper Methods
  private int? UserId(ClaimsPrincipal? user)
  {
    var claim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    return int.TryParse(claim, out var id) ? id : null;
  }

  private string ActionFromMethod(string method) => method.ToUpper() switch
  {
    "POST" => "CREATE",
    "PUT" => "UPDATE",
    "PATCH" => "UPDATE",
    "DELETE" => "DELETE",
    "GET" => "VIEW",
    _ => method.ToUpper()
  };

  private string EntityTypeFromPath(PathString path)
  {
    var segments = path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries);
    return segments is { Length: >= 2 } ? segments[1] : "Unknown";
  }

  private string? EntityIdFromPath(PathString path)
  {
    var segments = path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries);
    return segments is { Length: >= 3 } && int.TryParse(segments[2], out _)
        ? segments[2]
        : null;
  }

  private string ModuleFromPath(PathString path)
  {
    var p = path.Value?.ToLower() ?? string.Empty;

    if (p.Contains("/systemadmin/")) return "SystemAdmin";
    if (p.Contains("/hr/")) return "HR";
    if (p.Contains("/crm/")) return "CRM";
    if (p.Contains("/dms/")) return "DMS";
    if (p.Contains("/authentication")) return "Authentication";

    return "General";
  }

  #endregion
}









//using System.Diagnostics;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Json;
//using Domain.Entities.Entities.System;
//using Infrastructure.Sql.Context;
//using Microsoft.AspNetCore.WebUtilities;

//namespace Presentation.Api.Middleware;

//public class EnhancedAuditMiddleware
//{
//	private readonly RequestDelegate _next;
//	private readonly ILogger<EnhancedAuditMiddleware> _logger;
//	private readonly IConfiguration _configuration;
//	private readonly bool _captureResponseBody;
//	private readonly int _maxCaptureBytes;
//	private readonly string[] _captureContentTypes;
//	private readonly int _tempFileLimitBytes = 10 * 1024 * 1024; // 10 MB

//	public EnhancedAuditMiddleware(RequestDelegate next, ILogger<EnhancedAuditMiddleware> logger, IConfiguration configuration)
//	{
//		_next = next;
//		_logger = logger;
//		_configuration = configuration;

//		_captureResponseBody = _configuration.Value("AuditLogging:CaptureResponseBody", false);
//		_maxCaptureBytes = _configuration.Value("AuditLogging:MaxBodyCaptureBytes", 4096);
//		_captureContentTypes = _configuration.Section("AuditLogging:CaptureContentTypes").<string[]>() ?? new[] { "application/json", "text/" };
//	}

//	public async Task InvokeAsync(HttpContext context, CrmContext dbContext)
//	{
//		// Check if audit is enabled
//		if (!_configuration.Value<bool>("AuditLogging:EnableAuditMiddleware", true))
//		{
//			await _next(context);
//			return;
//		}

//		// Skip audit for certain paths
//		if (ShouldSkipAudit(context.Request.Path))
//		{
//			await _next(context);
//			return;
//		}

//		var stopwatch = Stopwatch.StartNew();

//		// Reuse correlation id if set by CorrelationIdMiddleware
//		var correlationId = context.Items["CorrelationId"] as string
//							?? context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
//							?? Activity.Current?.TraceId.ToString()
//							?? context.TraceIdentifier;

//		var originalBodyStream = context.Response.Body;

//		try
//		{
//			// Capture request body for POST/PUT/PATCH (only if JSON and small enough)
//			string? requestBody = null;
//			if (context.Request.Method != "GET" && context.Request.ContentType?.Contains("application/json") == true
//				&& context.Request.ContentLength.HasValue && context.Request.ContentLength <= _maxCaptureBytes)
//			{
//				context.Request.EnableBuffering();
//				requestBody = await new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
//				context.Request.Body.Position = 0;
//			}

//			if (_captureResponseBody && ShouldCaptureResponseByRequest(context.Request))
//			{
//				// Buffer response using file-backed buffering to avoid OOM
//				using var bufferingStream = new FileBufferingWriteStream(originalBodyStream, _maxCaptureBytes, _tempFileLimitBytes);
//				context.Response.Body = bufferingStream;

//				await _next(context);

//				stopwatch.Stop();

//				bufferingStream.Seek(0, SeekOrigin.Begin);

//				// OPTIONAL: read small response body for audit if it fits
//				string? responseText = null;
//				if (bufferingStream.CanSeek && bufferingStream.Length <= _maxCaptureBytes && IsTextBasedContentType(context.Response.ContentType))
//				{
//					responseText = await new StreamReader(bufferingStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true).ReadToEndAsync();
//					bufferingStream.Position = 0;
//				}

//				// Create audit log
//				var auditLog = CreateAuditLog(context, requestBody, stopwatch.ElapsedMilliseconds, correlationId);
//				// Optionally set a captured small response
//				if (!string.IsNullOrEmpty(responseText))
//				{
//					auditLog.OldValue = responseText; // or another property you keep for response
//				}

//				// Save audit log asynchronously (consider bounded queue instead of Task.Run in future)
//				_ = Task.Run(async () =>
//				{
//					try
//					{
//						using var scope = context.RequestServices.CreateScope();
//						var scopedContext = scope.ServiceProvider.RequiredService<CrmContext>();
//						scopedContext.AuditLogs.Add(auditLog);
//						await scopedContext.SaveChangesAsync();
//					}
//					catch (Exception ex)
//					{
//						_logger.LogError(ex, "Failed to save audit log for {Path}", context.Request.Path);
//					}
//				});

//				// Copy buffered response back to original
//				bufferingStream.Seek(0, SeekOrigin.Begin);
//				await bufferingStream.CopyToAsync(originalBodyStream);
//			}
//			else
//			{
//				// No response buffering
//				await _next(context);

//				stopwatch.Stop();

//				var auditLog = CreateAuditLog(context, requestBody, stopwatch.ElapsedMilliseconds, correlationId);

//				_ = Task.Run(async () =>
//				{
//					try
//					{
//						using var scope = context.RequestServices.CreateScope();
//						var scopedContext = scope.ServiceProvider.RequiredService<CrmContext>();
//						scopedContext.AuditLogs.Add(auditLog);
//						await scopedContext.SaveChangesAsync();
//					}
//					catch (Exception ex)
//					{
//						_logger.LogError(ex, "Failed to save audit log for {Path}", context.Request.Path);
//					}
//				});
//			}
//		}
//		catch (Exception ex)
//		{
//			stopwatch.Stop();
//			_logger.LogError(ex, "Error in audit middleware");

//			var errorAuditLog = CreateErrorAuditLog(context, ex, stopwatch.ElapsedMilliseconds, correlationId);
//			_ = Task.Run(async () =>
//			{
//				try
//				{
//					using var scope = context.RequestServices.CreateScope();
//					var scopedContext = scope.ServiceProvider.RequiredService<CrmContext>();
//					scopedContext.AuditLogs.Add(errorAuditLog);
//					await scopedContext.SaveChangesAsync();
//				}
//				catch { }
//			});

//			throw;
//		}
//		finally
//		{
//			context.Response.Body = originalBodyStream;
//		}
//	}

//	private bool ShouldCaptureResponseByRequest(HttpRequest request)
//	{
//		// Avoid capturing for static assets and non-text responses
//		if (!IsTextBasedContentType(request.ContentType)) return false;

//		var path = request.Path.Value?.ToLower() ?? string.Empty;
//		var skipPrefixes = new[] { "/health", "/swagger", "/uploads", "/_framework" };
//		if (skipPrefixes.Any(p => path.StartsWith(p))) return false;

//		return true;
//	}

//	private bool IsTextBasedContentType(string contentType)
//	{
//		if (string.IsNullOrEmpty(contentType)) return false;
//		contentType = contentType.ToLower();
//		return _captureContentTypes.Any(ct => contentType.StartsWith(ct));
//	}

//	private bool ShouldSkipAudit(PathString path)
//	{
//		var pathValue = path.Value?.ToLower() ?? string.Empty;
//		var skipPaths = _configuration.Section("AuditLogging:SkipPaths").<string[]>() ?? Array.Empty<string>();

//		foreach (var skipPath in skipPaths)
//		{
//			if (pathValue.Contains(skipPath.ToLower()))
//				return true;
//		}

//		return false;
//	}

//	// Defensive session access inside CreateAuditLog
//	private AuditLog CreateAuditLog(HttpContext context, string? requestBody, long durationMs, string correlationId)
//	{
//		var user = context.User;
//		var request = context.Request;
//		var response = context.Response;

//		// SAFE session access: check feature first
//		string? sessionId = null;
//		var sessionFeature = context.Features.<Microsoft.AspNetCore.HttpGet.Features.ISessionFeature>();
//		if (sessionFeature != null)
//		{
//			try
//			{
//				sessionId = context.Session?.Id;
//			}
//			catch
//			{
//				sessionId = null;
//			}
//		}

//		return new AuditLog
//		{
//			UserId = UserId(user),
//			Username = user?.Identity?.Name ?? "Anonymous",
//			IpAddress = context.Connection.RemoteIpAddress?.ToString(),
//			UserAgent = request.Headers["User-Agent"].FirstOrDefault(),

//			Action = ActionFromMethod(request.Method),
//			EntityType = EntityTypeFromPath(request.Path),
//			EntityId = EntityIdFromPath(request.Path),
//			Endpoint = $"{request.Method} {request.Path}",
//			Module = ModuleFromPath(request.Path),

//			NewValue = requestBody,

//			Timestamp = DateTime.UtcNow,

//			CorrelationId = correlationId,
//			SessionId = sessionId,
//			RequestId = context.TraceIdentifier,

//			Success = response.StatusCode >= 200 && response.StatusCode < 300,
//			StatusCode = response.StatusCode,
//			DurationMs = (int)durationMs
//		};
//	}

//	private AuditLog CreateErrorAuditLog(HttpContext context, Exception ex, long durationMs, string correlationId)
//	{
//		var auditLog = CreateAuditLog(context, null, durationMs, correlationId);
//		auditLog.Success = false;
//		auditLog.StatusCode = 500;
//		auditLog.ErrorMessage = $"{ex.Type().Name}: {ex.Message}";
//		return auditLog;
//	}

//	private int? UserId(ClaimsPrincipal? user)
//	{
//		var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//		return int.TryParse(userIdClaim, out var userId) ? userId : null;
//	}

//	private string ActionFromMethod(string method)
//	{
//		return method.ToUpper() switch
//		{
//			"POST" => "CREATE",
//			"PUT" => "UPDATE",
//			"PATCH" => "UPDATE",
//			"DELETE" => "DELETE",
//			"GET" => "VIEW",
//			_ => method.ToUpper()
//		};
//	}

//	private string EntityTypeFromPath(PathString path)
//	{
//		var segments = path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries);
//		if (segments != null && segments.Length >= 2)
//		{
//			return segments[1];
//		}
//		return "Unknown";
//	}

//	private string? EntityIdFromPath(PathString path)
//	{
//		var segments = path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries);
//		if (segments != null && segments.Length >= 3 && int.TryParse(segments[2], out _))
//		{
//			return segments[2];
//		}
//		return null;
//	}

//	private string ModuleFromPath(PathString path)
//	{
//		var pathValue = path.Value?.ToLower() ?? string.Empty;

//		if (pathValue.Contains("/systemadmin/")) return "SystemAdmin";
//		if (pathValue.Contains("/hr/")) return "HR";
//		if (pathValue.Contains("/crm/")) return "CRM";
//		if (pathValue.Contains("/dms/")) return "DMS";
//		if (pathValue.Contains("/authentication")) return "Authentication";

//		return "General";
//	}
//}