using bdDevCRM.Api.Middleware.Shared;
using Serilog.Context;
using System.Diagnostics;

namespace Presentation.Api.Middleware;

/// <summary>
/// Creates PipelineContext, resolves CorrelationId, starts Stopwatch.
/// All other middleware read from PipelineContext — no duplication.
/// </summary>
public class CorrelationIdMiddleware
{
	private readonly RequestDelegate _next;
	private const string CorrelationIdHeader = "X-Correlation-ID";

	public CorrelationIdMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		// Create PipelineContext (Stopwatch starts automatically)
		var pipelineCtx = RequestPipelineContext.OrCreate(context);

		// Resolve Correlation ID: incoming header → Activity → TraceIdentifier → GUID
		var incoming = context.Request.Headers[CorrelationIdHeader].FirstOrDefault();
		var correlationId = incoming
							?? Activity.Current?.Id
							?? context.TraceIdentifier
							?? Guid.NewGuid().ToString();

		// Store in PipelineContext (single source of truth)
		pipelineCtx.CorrelationId = correlationId;

		// Also store in well-known locations for compatibility
		context.Items["CorrelationId"] = correlationId;
		context.TraceIdentifier = correlationId;

		// Enrich Activity for distributed tracing
		if (Activity.Current == null)
		{
			var activity = new Activity("http-request");
			activity.Start();
			Activity.Current = activity;
		}

		try { Activity.Current?.AddBaggage("CorrelationId", correlationId); }
		catch { /* swallow */ }

		// Response header — use OnStarting to avoid "headers already sent" error
		context.Response.OnStarting(() =>
		{
			context.Response.Headers[CorrelationIdHeader] = correlationId;
			return Task.CompletedTask;
		});

		// Serilog enrichment
		using (LogContext.PushProperty("CorrelationId", correlationId))
		{
			await _next(context);
		}
	}
}