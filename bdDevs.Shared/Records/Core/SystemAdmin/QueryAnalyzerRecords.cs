namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new query analyzer entry.
/// </summary>
public record CreateQueryAnalyzerRecord(
    string? ReportHeader,
    string? ReportTitle);

/// <summary>
/// Record for updating an existing query analyzer entry.
/// </summary>
public record UpdateQueryAnalyzerRecord(
    int ReportHeaderId,
    string? ReportHeader,
    string? ReportTitle);

/// <summary>
/// Record for deleting a query analyzer entry.
/// </summary>
public record DeleteQueryAnalyzerRecord(int ReportHeaderId);
