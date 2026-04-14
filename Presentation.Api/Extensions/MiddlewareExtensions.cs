using Presentation.Api.Middleware;
using Presentation.Api.Middleware;

namespace Presentation.Api.Extensions;

public static class MiddlewareExtensions
{
  /// <summary>
  /// Middleware pipeline — order matters!
  /// </summary>
  public static void UseApiMiddleware(
      this WebApplication app,
      IConfiguration configuration)
  {
    // [1] Exception handler — সবচেয়ে বাইরে
    app.UseMiddleware<StandardExceptionMiddleware>();

    // [2] CorrelationId + PipelineContext + Stopwatch
    app.UseMiddleware<CorrelationIdMiddleware>();

    // [3] Performance monitoring
    app.UseMiddleware<PerformanceMonitoringMiddleware>();

    // [4] Structured logging
    app.UseMiddleware<StructuredLoggingMiddleware>();

    // [5] Response compression
    app.UseResponseCompression();

    // [6] HTTPS redirect
    app.UseHttpsRedirection();

    // [7] CORS
    app.UseCors("CorsPolicy");

    // [8] Static files
    app.UseStaticFiles();

    // [9] Cookie policy
    app.UseCookiePolicy();

    // [10] Authentication
    app.UseAuthentication();

    // [11] Token blacklist — auth-এর পরে
    app.UseMiddleware<TokenBlacklistMiddleware>();

    // [12] Authorization
    app.UseAuthorization();

    // [13] Audit — user context পাওয়ার পরে
    if (configuration.GetValue<bool>(
            "AuditLogging:EnableAuditMiddleware", true))
    {
      app.UseMiddleware<EnhancedAuditMiddleware>();
    }
  }
}