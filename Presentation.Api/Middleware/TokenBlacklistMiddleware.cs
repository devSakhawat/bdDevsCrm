using Presentation.Api.Middleware.Shared;
using Domain.Contracts.Services;
using System.Net;
using System.Text.Json;

namespace Presentation.Api.Middleware;

/// <summary>
/// Checks every authenticated request against the token blacklist.
/// Runs AFTER Authentication middleware so User claims are available.
/// Runs BEFORE Authorization middleware so blacklisted tokens never reach controllers.
///
/// কেন এই middleware দরকার?
/// JWT token stateless — একবার issue হলে expiry পর্যন্ত valid থাকে।
/// User logout করলে বা token revoke হলে এই middleware সেটা reject করে।
/// </summary>
public class TokenBlacklistMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<TokenBlacklistMiddleware> _logger;

  // SkipPaths — এই paths-এ token check করা হবে না
  private static readonly HashSet<string> _skipPaths = new(
      StringComparer.OrdinalIgnoreCase)
    {
        "/api/authentication/login",
        "/api/authentication/refresh-token",
        "/api/authentication/register",
        "/swagger",
        "/health",
    };

  public TokenBlacklistMiddleware(
      RequestDelegate next,
      ILogger<TokenBlacklistMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    // ── ১. Skip করো যদি authenticated না হয় ──────────────────
    // Anonymous request — check করার দরকার নেই
    if (!context.User.Identity?.IsAuthenticated ?? true)
    {
      await _next(context);
      return;
    }

    // ── ২. Skip করো নির্দিষ্ট paths-এ ────────────────────────
    var path = context.Request.Path.Value ?? string.Empty;
    if (ShouldSkip(path))
    {
      await _next(context);
      return;
    }

    // ── ৩. Token extract করো ──────────────────────────────────
    var token = ExtractToken(context);
    if (string.IsNullOrEmpty(token))
    {
      await _next(context);
      return;
    }

    // ── ৪. Blacklist check করো ────────────────────────────────
    // ITokenBlacklistService Scoped — middleware Singleton হতে পারে না
    // তাই IServiceScopeFactory দিয়ে scope তৈরি করো
    using var scope = context.RequestServices.CreateScope();
    var blacklistService = scope.ServiceProvider
        .GetRequiredService<IServiceManager>()
        .TokenBlacklist;

    bool isBlacklisted = await blacklistService
        .IsTokenBlacklistedAsync(token);

    if (isBlacklisted)
    {
      var pipelineCtx = RequestPipelineContext.Get(context);
      var correlationId = pipelineCtx?.CorrelationId
          ?? context.TraceIdentifier;

      _logger.LogWarning(
          "Blacklisted token rejected. " +
          "Path: {Path} | IP: {IP} | CorrelationId: {CorrelationId}",
          path,
          context.Connection.RemoteIpAddress?.ToString(),
          correlationId);

      await WriteUnauthorizedResponse(context, correlationId);
      return; // pipeline বন্ধ করো
    }

    // ── ৫. Token valid — পরের middleware-এ যাও ───────────────
    await _next(context);
  }

  // ─────────────────────────────────────────────────────────────
  // Helpers
  // ─────────────────────────────────────────────────────────────

  private static bool ShouldSkip(string path)
  {
    foreach (var skip in _skipPaths)
    {
      if (path.StartsWith(skip, StringComparison.OrdinalIgnoreCase))
        return true;
    }
    return false;
  }

  private static string? ExtractToken(HttpContext context)
  {
    // Authorization: Bearer <token>
    var authHeader = context.Request.Headers["Authorization"]
        .FirstOrDefault();

    if (string.IsNullOrEmpty(authHeader))
      return null;

    if (!authHeader.StartsWith("Bearer ",
            StringComparison.OrdinalIgnoreCase))
      return null;

    return authHeader["Bearer ".Length..].Trim();
  }

  private static async Task WriteUnauthorizedResponse(
      HttpContext context,
      string correlationId)
  {
    context.Response.StatusCode =
        (int)HttpStatusCode.Unauthorized;
    context.Response.ContentType = "application/json";

    var response = new
    {
      statusCode = 401,
      success = false,
      message = "Your session has been revoked. Please log in again.",
      correlationId,
      error = new
      {
        code = "TOKEN_REVOKED",
        type = "TokenRevoked",
      }
    };

    var json = JsonSerializer.Serialize(response,
        new JsonSerializerOptions
        {
          PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

    await context.Response.WriteAsync(json);
  }
}