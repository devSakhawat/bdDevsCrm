namespace Presentation.Mvc.Middleware;

/// <summary>
/// Middleware to check if user should be redirected to login page
/// For MVC app with client-side token-based authentication
/// </summary>
public class AuthenticationCheckMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationCheckMiddleware> _logger;

    // Public routes that don't require authentication
    private static readonly string[] PublicPaths = new[]
    {
        "/account/login",
        "/lib/",
        "/css/",
        "/js/",
        "/images/",
        "/favicon.ico"
    };

    public AuthenticationCheckMiddleware(RequestDelegate next, ILogger<AuthenticationCheckMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;

        // Skip middleware for public paths
        if (IsPublicPath(path))
        {
            await _next(context);
            return;
        }

        // For client-side token-based auth, we rely on JavaScript to redirect
        // This middleware is optional and can be used for server-side route protection if needed
        // Currently, we just pass through to the next middleware
        await _next(context);
    }

    private static bool IsPublicPath(string path)
    {
        return PublicPaths.Any(publicPath => path.StartsWith(publicPath, StringComparison.OrdinalIgnoreCase));
    }
}

/// <summary>
/// Extension method to add the middleware to the pipeline
/// </summary>
public static class AuthenticationCheckMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthenticationCheck(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthenticationCheckMiddleware>();
    }
}
