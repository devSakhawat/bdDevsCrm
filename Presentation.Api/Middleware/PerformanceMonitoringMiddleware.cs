using bdDevCRM.Api.Middleware.Shared;

namespace Presentation.Api.Middleware;

/// <summary>
/// Measures request execution time and detects slow requests.
/// 
/// </summary>
public class PerformanceMonitoringMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<PerformanceMonitoringMiddleware> _logger;
	private readonly int _slowRequestThresholdMs;
	private readonly int _verySlowRequestThresholdMs;

	public PerformanceMonitoringMiddleware(RequestDelegate next, ILogger<PerformanceMonitoringMiddleware> logger, IConfiguration configuration)
	{
		_next = next;
		_logger = logger;
		_slowRequestThresholdMs = configuration.GetValue<int>("PerformanceMonitoring:SlowRequestThresholdMs", 1000);
		_verySlowRequestThresholdMs = configuration.GetValue<int>("PerformanceMonitoring:VerySlowRequestThresholdMs", 5000);
	}

	public async Task InvokeAsync(HttpContext context)
	{
		// Use shared stopwatch (do not create a new one)
		var pipelineCtx = RequestPipelineContext.OrCreate(context);
		var requestPath = context.Request.Path.Value;
		var requestMethod = context.Request.Method;

		// Conflict #5 Fix: Set header using OnStarting()
		// This will execute just before response body writing starts
		// so there will be no "Headers already sent" error
		context.Response.OnStarting(() =>
		{
			var elapsed = pipelineCtx.Stopwatch.ElapsedMilliseconds;
			context.Response.Headers["X-Response-Time-Ms"] = elapsed.ToString();
			return Task.CompletedTask;
		});

		try
		{
			await _next(context);

			// Read time from shared stopwatch (do not stop it — others will use it too)
			var elapsedMs = pipelineCtx.Stopwatch.ElapsedMilliseconds;

			if (elapsedMs >= _verySlowRequestThresholdMs)
			{
				_logger.LogWarning( "VERY SLOW REQUEST: {Method} {Path} took {Duration}ms (Status: {StatusCode}) | CorrelationId: {CorrelationId}",
					requestMethod, requestPath, elapsedMs, context.Response.StatusCode,
					pipelineCtx.CorrelationId);
			}
			else if (elapsedMs >= _slowRequestThresholdMs)
			{
				_logger.LogWarning(
					"SLOW REQUEST: {Method} {Path} took {Duration}ms (Status: {StatusCode}) | CorrelationId: {CorrelationId}",
					requestMethod, requestPath, elapsedMs, context.Response.StatusCode,
					pipelineCtx.CorrelationId);
			}
			else
			{
				_logger.LogDebug(
					"Request: {Method} {Path} completed in {Duration}ms (Status: {StatusCode})",
					requestMethod, requestPath, elapsedMs, context.Response.StatusCode);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex,
				"Request FAILED: {Method} {Path} after {Duration}ms | CorrelationId: {CorrelationId}",
				requestMethod, requestPath, pipelineCtx.Stopwatch.ElapsedMilliseconds,
				pipelineCtx.CorrelationId);
			throw;
		}
	}
}



