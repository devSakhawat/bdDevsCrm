namespace bdDevs.Shared;

/// <summary>
/// Standardized API response wrapper for all endpoints
/// Provides consistent structure for both success and error responses
/// </summary>
public class ApiResponse<T>
{
    /// <summary>
    /// HTTP status code (200, 201, 400, 404, etc.)
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Indicates if the request was successful (2xx status codes)
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Human-readable message describing the result
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// API version information
    /// </summary>
    public string Version { get; set; } = "1.0";

    /// <summary>
    /// Timestamp of the response (UTC)
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Response payload data (null for errors)
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// Error details (null for successful responses)
    /// </summary>
    public ErrorDetails Error { get; set; }

    /// <summary>
    /// Pagination metadata (for list responses)
    /// </summary>
    public PaginationMetadata Pagination { get; set; }

    /// <summary>
    /// HATEOAS links for resource discovery
    /// </summary>
    public List<ResourceLink> Links { get; set; }

    /// <summary>
    /// Correlation ID for request tracing
    /// </summary>
    public string CorrelationId { get; set; }
}

/// <summary>
/// Non-generic version for responses without data payload
/// </summary>
public class ApiResponse : ApiResponse<object>
{
}

/// <summary>
/// Error details structure
/// </summary>
public class ErrorDetails
{
    /// <summary>
    /// Error code for programmatic handling (e.g., "VALIDATION_ERROR", "NOT_FOUND")
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Error type/category
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Detailed error message
    /// </summary>
    public string Details { get; set; }

    /// <summary>
    /// Field-level validation errors
    /// </summary>
    public Dictionary<string, string[]> ValidationErrors { get; set; }

    /// <summary>
    /// Stack trace (development mode only)
    /// </summary>
    public string StackTrace { get; set; }

    /// <summary>
    /// Additional error context
    /// </summary>
    public Dictionary<string, object> AdditionalData { get; set; }
}

/// <summary>
/// Pagination metadata for list responses
/// </summary>
public class PaginationMetadata
{
    /// <summary>
    /// Current page number (1-based)
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items across all pages
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Indicates if there is a next page
    /// </summary>
    public bool HasNextPage { get; set; }

    /// <summary>
    /// Indicates if there is a previous page
    /// </summary>
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// Index of the first item on current page (0-based)
    /// </summary>
    public int StartIndex { get; set; }

    /// <summary>
    /// Index of the last item on current page (0-based)
    /// </summary>
    public int EndIndex { get; set; }
}

/// <summary>
/// HATEOAS link for resource discovery
/// </summary>
public class ResourceLink
{
    /// <summary>
    /// Link relation type (self, next, prev, related, etc.)
    /// </summary>
    public string Rel { get; set; }

    /// <summary>
    /// Target URL
    /// </summary>
    public string Href { get; set; }

    /// <summary>
    /// HTTP method (GET, POST, PUT, DELETE, etc.)
    /// </summary>
    public string Method { get; set; } = "GET";

    /// <summary>
    /// Link description
    /// </summary>
    public string Description { get; set; }
}
