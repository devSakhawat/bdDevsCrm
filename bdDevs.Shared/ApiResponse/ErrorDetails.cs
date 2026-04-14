namespace bdDevs.Shared;

/// <summary>
/// Legacy ErrorDetails kept for backwards compatibility.
/// Prefer using the ErrorDetails class in ApiResponse.cs for new code.
/// </summary>
public class LegacyErrorDetails
{
  public int StatusCode { get; set; }
  public string? Message { get; set; }
  public string? ErrorType { get; set; }

  public override string ToString() => System.Text.Json.JsonSerializer.Serialize(this);
}