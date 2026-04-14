//namespace bdDevCRM.Api.Middleware;

///// <summary>
///// Middleware for adding HTTP caching headers to responses
///// Supports Cache-Control, ETag, and Last-Modified headers
///// </summary>
//public class CacheHeaderMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<CacheHeaderMiddleware> _logger;

//    public CacheHeaderMiddleware(RequestDelegate next, ILogger<CacheHeaderMiddleware> logger)
//    {
//        _next = next;
//        _logger = logger;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        // Store original body stream
//        var originalBodyStream = context.Response.Body;

//        try
//        {
//            using var memoryStream = new MemoryStream();
//            context.Response.Body = memoryStream;

//            // Call the next middleware
//            await _next(context);

//            // Add cache headers based on request path and method
//            AddCacheHeaders(context);

//            // Copy response to original stream
//            memoryStream.Seek(0, SeekOrigin.Begin);
//            await memoryStream.CopyToAsync(originalBodyStream);
//        }
//        finally
//        {
//            context.Response.Body = originalBodyStream;
//        }
//    }

//    private void AddCacheHeaders(HttpContext context)
//    {
//        var path = context.Request.Path.Value?.ToLower() ?? "";
//        var method = context.Request.Method;
//        var statusCode = context.Response.StatusCode;

//        // Only add cache headers for successful GET requests
//        if (method != "GET" || statusCode < 200 || statusCode >= 300)
//        {
//            // For non-GET or error responses, prevent caching
//            context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
//            context.Response.Headers["Pragma"] = "no-cache";
//            context.Response.Headers["Expires"] = "0";
//            return;
//        }

//        // Determine cache strategy based on endpoint
//        if (IsStaticResource(path))
//        {
//            // Static resources: Cache for 1 year
//            context.Response.Headers["Cache-Control"] = "public, max-age=31536000, immutable";
//        }
//        else if (IsPublicEndpoint(path))
//        {
//            // Public data: Cache for 5 minutes with revalidation
//            context.Response.Headers["Cache-Control"] = "public, max-age=300, must-revalidate";
//            AddETagHeader(context);
//        }
//        else if (IsUserSpecificEndpoint(path))
//        {
//            // User-specific data: Cache privately for 1 minute
//            context.Response.Headers["Cache-Control"] = "private, max-age=60, must-revalidate";
//            AddETagHeader(context);
//        }
//        else if (IsDynamicEndpoint(path))
//        {
//            // Dynamic data: No cache
//            context.Response.Headers["Cache-Control"] = "no-cache, must-revalidate";
//            AddETagHeader(context);
//        }
//        else
//        {
//            // Default: Cache for 1 minute with revalidation
//            context.Response.Headers["Cache-Control"] = "public, max-age=60, must-revalidate";
//            AddETagHeader(context);
//        }

//        // Add Last-Modified header
//        context.Response.Headers["Last-Modified"] = DateTime.UtcNow.ToString("R");
//    }

//    private void AddETagHeader(HttpContext context)
//    {
//        // Generate ETag based on response content hash
//        if (context.Response.Body.CanSeek && context.Response.Body.Length > 0)
//        {
//            var position = context.Response.Body.Position;
//            context.Response.Body.Seek(0, SeekOrigin.Begin);

//            using var md5 = System.Security.Cryptography.MD5.Create();
//            var hash = md5.ComputeHash(context.Response.Body);
//            var etag = $"\"{Convert.ToBase64String(hash)}\"";

//            context.Response.Headers["ETag"] = etag;
//            context.Response.Body.Seek(position, SeekOrigin.Begin);

//            // Check If-None-Match header for conditional requests
//            if (context.Request.Headers.TryGetValue("If-None-Match", out var ifNoneMatch))
//            {
//                if (ifNoneMatch == etag)
//                {
//                    context.Response.StatusCode = StatusCodes.Status304NotModified;
//                    context.Response.Body.SetLength(0);
//                }
//            }
//        }
//    }

//    private bool IsStaticResource(string path)
//    {
//        var staticExtensions = new[] { ".css", ".js", ".jpg", ".jpeg", ".png", ".gif", ".ico", ".svg", ".woff", ".woff2", ".ttf" };
//        return staticExtensions.Any(ext => path.EndsWith(ext));
//    }

//    private bool IsPublicEndpoint(string path)
//    {
//        // Public endpoints that can be cached aggressively
//        var publicPaths = new[] { "/api/public", "/api/content", "/api/static" };
//        return publicPaths.Any(p => path.StartsWith(p));
//    }

//    private bool IsUserSpecificEndpoint(string path)
//    {
//        // User-specific endpoints
//        var userPaths = new[] { "/api/users/me", "/api/profile", "/api/dashboard" };
//        return userPaths.Any(p => path.StartsWith(p));
//    }

//    private bool IsDynamicEndpoint(string path)
//    {
//        // Highly dynamic endpoints that should not be cached
//        var dynamicPaths = new[] { "/api/realtime", "/api/notifications", "/api/events" };
//        return dynamicPaths.Any(p => path.StartsWith(p));
//    }
//}
