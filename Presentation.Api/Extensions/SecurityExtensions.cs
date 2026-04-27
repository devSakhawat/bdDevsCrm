using System.Threading.RateLimiting;

namespace Presentation.Api.Extensions;

/// <summary>
/// Security hardening extensions: security response headers and API rate limiting.
/// </summary>
public static class SecurityExtensions
{
    /// <summary>
    /// Adds sliding-window rate limiting to the DI container.
    /// Limits each client (by IP) to 120 requests per minute globally and
    /// 20 requests per minute on authentication endpoints.
    /// </summary>
    public static void ConfigureRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // Global policy — applied to all routes by default
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetSlidingWindowLimiter(clientIp, _ =>
                    new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 120,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 6,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });

            // Strict policy for authentication endpoints
            options.AddPolicy("AuthPolicy", context =>
            {
                var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetSlidingWindowLimiter($"auth:{clientIp}", _ =>
                    new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 20,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 6,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });
        });
    }

    /// <summary>
    /// Adds standard security response headers to every HTTP response.
    /// Headers added: X-Content-Type-Options, X-Frame-Options, Referrer-Policy,
    /// X-XSS-Protection, Permissions-Policy, and Cache-Control for API responses.
    /// </summary>
    public static void UseSecurityHeaders(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            var headers = context.Response.Headers;

            // Prevent MIME-type sniffing
            headers["X-Content-Type-Options"] = "nosniff";

            // Prevent clickjacking
            headers["X-Frame-Options"] = "DENY";

            // Control referrer information
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

            // Disable legacy XSS filter (modern browsers handle this via CSP)
            headers["X-XSS-Protection"] = "0";

            // Restrict browser feature access
            headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), payment=()";

            // No caching for API responses
            if (context.Request.Path.StartsWithSegments("/bdDevs-crm"))
            {
                headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
                headers["Pragma"] = "no-cache";
            }

            await next();
        });
    }
}
