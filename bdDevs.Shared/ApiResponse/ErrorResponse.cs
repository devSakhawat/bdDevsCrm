public class ErrorResponse
{
    public string ErrorCode { get; set; }
    public string Message { get; set; }
    public string UserFriendlyMessage { get; set; }
    public string CorrelationId { get; set; }
    public DateTime Timestamp { get; set; }
    public string TraceId { get; set; }
    public List<ValidationError> ValidationErrors { get; set; } = new();
    public Dictionary<string, object> AdditionalData { get; set; } = new();
    public string Details { get; set; } // Only in development
}

public class ValidationError
{
    public string Field { get; set; }
    public string Message { get; set; }
    public string ErrorCode { get; set; }
}