namespace Domain.Entities.Entities.System;

/// <summary>
/// Represents a query analyzer result used for customized report listings.
/// Populated from raw SQL queries against ReportBuilder table.
/// </summary>
public class QueryAnalyzer
{
    public int ReportHeaderId { get; set; }
    public string? ReportHeader { get; set; }
    public string? ReportTitle { get; set; }
}
