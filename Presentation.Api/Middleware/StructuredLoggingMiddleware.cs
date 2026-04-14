using Microsoft.AspNetCore.Mvc.Controllers;

namespace Presentation.Api.Middleware;

/// <summary>
/// Structured request/response logging middleware.
/// 
/// </summary>
public class StructuredLoggingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<StructuredLoggingMiddleware> _logger;
  private readonly bool _isEnabled;
  private readonly bool _logRequestBody;
  private readonly bool _logResponseBody;
  private readonly int _maxBodySize;

  public StructuredLoggingMiddleware(RequestDelegate next, ILogger<StructuredLoggingMiddleware> logger, IConfiguration configuration)
  {
    _next = next;
    _logger = logger;
    _isEnabled = configuration.GetValue("Logging:StructuredLogging:Enabled", true);
    _logRequestBody = configuration.GetValue("Logging:StructuredLogging:LogRequestBody", true);
    _logResponseBody = configuration.GetValue("Logging:StructuredLogging:LogResponseBody", false);
    _maxBodySize = configuration.GetValue("Logging:StructuredLogging:MaxBodySize", 4096);
  }

  public async Task InvokeAsync(HttpContext context)
  {
    if (!_isEnabled)
    {
      await _next(context);
      return;
    }

    //  everything from shared context — do not create new instances
    var pipelineCtx = RequestPipelineContext.OrCreate(context);

    // Read request body only once (will be cached)
    string? requestBody = null;
    if (_logRequestBody && context.Request.Method != "GET")
    {
      requestBody = await RequestBodyReader.ReadOnceAsync(context, _maxBodySize);

      // Mask sensitive data
      if (!string.IsNullOrEmpty(requestBody))
      {
        requestBody = MaskSensitiveData(requestBody);
      }
    }

    try
    {
      await _next(context);
    }
    finally
    {
      // Read elapsed time from shared stopwatch
      var elapsedMs = pipelineCtx.Stopwatch.ElapsedMilliseconds;

      //  Controller/Action name from endpoint metadata
      var endpoint = context.GetEndpoint();
      var actionDescriptor = endpoint?.Metadata
          .GetMetadata<ControllerActionDescriptor>();
      var controllerName = actionDescriptor?.ControllerName;
      var actionName = actionDescriptor?.ActionName;

      // Determine log level
      var logLevel = context.Response.StatusCode >= 500 ? LogLevel.Error :
                     context.Response.StatusCode >= 400 ? LogLevel.Warning :
                     elapsedMs > 5000 ? LogLevel.Warning :
                     LogLevel.Information;

      // Main log with Controller.Action info + shared correlation ID
      _logger.Log(logLevel,
          "HTTP {Method} {Path} → {Controller}.{Action} responded {StatusCode} in {Duration}ms | CorrelationId: {CorrelationId}",
          context.Request.Method,
          context.Request.Path.Value,
          controllerName ?? "N/A",
          actionName ?? "N/A",
          context.Response.StatusCode,
          elapsedMs,
          pipelineCtx.CorrelationId);

      // Log details at Debug level
      if (_logger.IsEnabled(LogLevel.Debug))
      {
        _logger.LogDebug(
            "Request Details: {Method} {Scheme}://{Host}{Path}{Query} | " +
            "ContentType: {ContentType} | RemoteIp: {RemoteIp} | " +
            "User: {User} | UserAgent: {UserAgent} | Body: {Body}",
            context.Request.Method,
            context.Request.Scheme,
            context.Request.Host,
            context.Request.Path,
            context.Request.QueryString,
            context.Request.ContentType,
            context.Connection.RemoteIpAddress?.ToString(),
            context.User?.Identity?.Name ?? "Anonymous",
            context.Request.Headers["User-Agent"].ToString(),
            requestBody ?? "[EMPTY]");
      }
    }
  }

  private string MaskSensitiveData(string content)
  {
    var sensitiveFields = new[] { "password", "token", "apikey", "secret", "authorization" };

    foreach (var field in sensitiveFields)
    {
      var pattern = $"\"{field}\"\\s*:\\s*\"([^\"]+)\"";
      content = System.Text.RegularExpressions.Regex.Replace(
          content,
          pattern,
          $"\"{field}\": \"[REDACTED]\"",
          System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    }

    return content;
  }
}






//using System.Diagnostics;
//using System.Text;
//using System.Text.Json;
//using Microsoft.AspNetCore.WebUtilities;

//namespace Presentation.Api.Middleware;

///// <summary>
///// Enhanced middleware for structured request/response logging
///// Captures detailed information for debugging and monitoring
///// Improvements:
///// - Reuse correlation id from CorrelationIdMiddleware (no new GUIDs)
///// - Response body capture guarded by config, content-type whitelist and max size
///// - Uses FileBufferingWriteStream to avoid unbounded MemoryStream growth
///// </summary>
//public class StructuredLoggingMiddleware
//{
//	private readonly RequestDelegate _next;
//	private readonly ILogger<StructuredLoggingMiddleware> _logger;
//	private readonly IConfiguration _configuration;
//	private readonly bool _isEnabled;
//	private readonly bool _logRequestBody;
//	private readonly bool _logResponseBody;
//	private readonly int _maxBodySize;
//	private readonly string[] _captureContentTypes;
//	private readonly int _tempFileLimitBytes = 10 * 1024 * 1024; // 10 MB temp file limit for buffering

//	public StructuredLoggingMiddleware(RequestDelegate next, ILogger<StructuredLoggingMiddleware> logger, IConfiguration configuration)
//	{
//		_next = next;
//		_logger = logger;
//		_configuration = configuration;

//		_isEnabled = _configuration.Value("Logging:StructuredLogging:Enabled", true);
//		_logRequestBody = _configuration.Value("Logging:StructuredLogging:LogRequestBody", true);
//		_logResponseBody = _configuration.Value("Logging:StructuredLogging:LogResponseBody", false);
//		_maxBodySize = _configuration.Value("Logging:StructuredLogging:MaxBodySize", 4096);
//		_captureContentTypes = _configuration.Section("Logging:StructuredLogging:CaptureContentTypes").<string[]>()
//							   ?? new[] { "application/json", "text/" };
//	}

//	public async Task InvokeAsync(HttpContext context)
//	{
//		if (!_isEnabled)
//		{
//			await _next(context);
//			return;
//		}

//		// Reuse correlation id produced by CorrelationIdMiddleware (if any)
//		var correlationId = context.Items["CorrelationId"] as string
//							?? context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
//							?? Activity.Current?.TraceId.ToString()
//							?? context.TraceIdentifier;

//		// Do not overwrite a pre-existing correlation id set earlier
//		if (context.Items["CorrelationId"] == null)
//			context.Items["CorrelationId"] = correlationId;

//		var stopwatch = Stopwatch.StartNew();
//		var requestLog = await CaptureRequest(context);

//		var originalBodyStream = context.Response.Body;

//		// Only buffer response if configured to do so
//		if (_logResponseBody && ShouldCaptureByRequest(context.Request))
//		{
//			// Use FileBufferingWriteStream to avoid unbounded memory usage
//			// NOTE: use named arguments to select the correct overload and avoid accidental overload resolution
//			using var bufferingStream = new FileBufferingWriteStream(memoryThreshold: _maxBodySize, bufferLimit: _tempFileLimitBytes);
//			context.Response.Body = bufferingStream;

//			try
//			{
//				await _next(context);
//			}
//			finally
//			{
//				stopwatch.Stop();

//				// Prepare response log using the buffered stream
//				bufferingStream.Seek(0, SeekOrigin.Begin);
//				var responseLog = await CaptureResponse(context, bufferingStream);

//				// Log structured data
//				LogRequestResponse(requestLog, responseLog, stopwatch.ElapsedMilliseconds, correlationId);

//				// Copy the buffered response back to the original stream
//				bufferingStream.Seek(0, SeekOrigin.Begin);
//				await bufferingStream.CopyToAsync(originalBodyStream);
//				context.Response.Body = originalBodyStream;
//			}
//		}
//		else
//		{
//			// No response buffering; minimal overhead path
//			try
//			{
//				await _next(context);
//			}
//			finally
//			{
//				stopwatch.Stop();

//				// Create response log without body capture
//				var responseLog = new ResponseLog
//				{
//					StatusCode = context.Response.StatusCode,
//					ContentType = context.Response.ContentType,
//					ContentLength = context.Response.ContentLength ?? 0,
//					Headers = CaptureHeaders(context.Response.Headers),
//					Body = _logResponseBody ? "[SKIPPED-BY-CONFIG]" : null
//				};

//				LogRequestResponse(requestLog, responseLog, stopwatch.ElapsedMilliseconds, correlationId);
//			}
//		}
//	}

//	private async Task<RequestLog> CaptureRequest(HttpContext context)
//	{
//		var request = context.Request;

//		var requestLog = new RequestLog
//		{
//			Method = request.Method,
//			Path = request.Path,
//			QueryString = request.QueryString.ToString(),
//			Scheme = request.Scheme,
//			Host = request.Host.ToString(),
//			ContentType = request.ContentType,
//			ContentLength = request.ContentLength,
//			Headers = CaptureHeaders(request.Headers),
//			UserAgent = request.Headers["User-Agent"].ToString(),
//			RemoteIp = context.Connection.RemoteIpAddress?.ToString(),
//			User = context.User?.Identity?.Name
//		};

//		// Capture request body if enabled, small enough and present
//		if (_logRequestBody && request.ContentLength.HasValue && request.ContentLength > 0 && request.ContentLength <= _maxBodySize && IsTextBasedContentType(request.ContentType))
//		{
//			request.EnableBuffering();
//			requestLog.Body = await ReadBodyAsync(request.Body);
//			request.Body.Seek(0, SeekOrigin.Begin);
//		}

//		return requestLog;
//	}

//	private async Task<ResponseLog> CaptureResponse(HttpContext context, Stream responseStream)
//	{
//		var response = context.Response;

//		var responseLog = new ResponseLog
//		{
//			StatusCode = response.StatusCode,
//			ContentType = response.ContentType,
//			ContentLength = responseStream.CanSeek ? responseStream.Length : 0,
//			Headers = CaptureHeaders(response.Headers),
//			Body = null
//		};

//		// Capture response body only if configured and content-type allowed and small enough
//		if (_logResponseBody && IsTextBasedContentType(response.ContentType))
//		{
//			try
//			{
//				if (responseStream.CanSeek && responseStream.Length <= _maxBodySize)
//				{
//					responseStream.Seek(0, SeekOrigin.Begin);
//					responseLog.Body = await ReadBodyAsync(responseStream);
//				}
//				else if (responseStream.CanSeek)
//				{
//					responseLog.Body = "[TRUNCATED-TOO-LARGE]";
//				}
//			}
//			catch (Exception ex)
//			{
//				_logger.LogWarning(ex, "Failed to capture response body");
//				responseLog.Body = "[ERROR_READING_BODY]";
//			}
//		}

//		return responseLog;
//	}

//	private Dictionary<string, string> CaptureHeaders(IHeaderDictionary headers)
//	{
//		var sensitiveHeaders = new[] { "authorization", "cookie", "x-api-key", "x-auth-token" };
//		var capturedHeaders = new Dictionary<string, string>();

//		foreach (var header in headers)
//		{
//			var key = header.Key.ToLower();
//			if (sensitiveHeaders.Contains(key))
//			{
//				capturedHeaders[header.Key] = "[REDACTED]";
//			}
//			else
//			{
//				capturedHeaders[header.Key] = header.Value.ToString();
//			}
//		}

//		return capturedHeaders;
//	}

//	private async Task<string> ReadBodyAsync(Stream body)
//	{
//		try
//		{
//			using var reader = new StreamReader(
//				body,
//				Encoding.UTF8,
//				detectEncodingFromByteOrderMarks: false,
//				bufferSize: 4096,
//				leaveOpen: true);

//			var bodyContent = await reader.ReadToEndAsync();

//			// Truncate if too large (defensive)
//			if (bodyContent.Length > _maxBodySize)
//			{
//				bodyContent = bodyContent.Substring(0, _maxBodySize) + "... [TRUNCATED]";
//			}

//			// Mask sensitive fields
//			bodyContent = MaskSensitiveData(bodyContent);

//			return bodyContent;
//		}
//		catch (Exception ex)
//		{
//			_logger.LogWarning(ex, "Failed to read body stream");
//			return "[ERROR READING BODY]";
//		}
//	}

//	private string MaskSensitiveData(string content)
//	{
//		var sensitiveFields = new[] { "password", "token", "apikey", "secret", "authorization" };

//		foreach (var field in sensitiveFields)
//		{
//			var pattern = $"\"{field}\"\\s*:\\s*\"([^\"]+)\"";
//			content = System.Text.RegularExpressions.Regex.Replace(
//				content,
//				pattern,
//				$"\"{field}\": \"[REDACTED]\"",
//				System.Text.RegularExpressions.RegexOptions.IgnoreCase);
//		}

//		return content;
//	}

//	private void LogRequestResponse(RequestLog request, ResponseLog response, long durationMs, string correlationId)
//	{
//		var logData = new
//		{
//			CorrelationId = correlationId,
//			Request = request,
//			Response = response,
//			DurationMs = durationMs,
//			Timestamp = DateTime.UtcNow
//		};

//		var logLevel = response.StatusCode >= 500 ? LogLevel.Error :
//					   response.StatusCode >= 400 ? LogLevel.Warning :
//					   durationMs > 5000 ? LogLevel.Warning :
//					   LogLevel.Information;

//		_logger.Log(logLevel,
//			"HTTP {Method} {Path} responded {StatusCode} in {Duration}ms | CorrelationId: {CorrelationId}",
//			request.Method, request.Path, response.StatusCode, durationMs, correlationId);

//		_logger.LogDebug("Request/Response Details: {LogData}",
//			JsonSerializer.Serialize(logData, new JsonSerializerOptions
//			{
//				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//				WriteIndented = true
//			}));
//	}

//	private bool ShouldCaptureByRequest(HttpRequest request)
//	{
//		// Heuristic: only capture response bodies for requests that likely return text/json
//		if (!IsTextBasedContentType(request.ContentType))
//			return false;

//		// You can also skip certain paths here (health, static, uploads)
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

//	private class RequestLog
//	{
//		public string Method { get; set; }
//		public string Path { get; set; }
//		public string QueryString { get; set; }
//		public string Scheme { get; set; }
//		public string Host { get; set; }
//		public string ContentType { get; set; }
//		public long? ContentLength { get; set; }
//		public Dictionary<string, string> Headers { get; set; }
//		public string UserAgent { get; set; }
//		public string RemoteIp { get; set; }
//		public string User { get; set; }
//		public string Body { get; set; }
//	}

//	private class ResponseLog
//	{
//		public int StatusCode { get; set; }
//		public string ContentType { get; set; }
//		public long ContentLength { get; set; }
//		public Dictionary<string, string> Headers { get; set; }
//		public string Body { get; set; }
//	}
//}