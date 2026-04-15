namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new report builder entry.
/// </summary>
public record CreateReportBuilderRecord(
    string? ReportHeader,
    string? ReportTitle,
    int? QueryType,
    string? QueryText,
    int? IsActive,
    int? CreatedBy,
    DateTime? CreatedDate,
    int? UpdatedBy,
    DateTime? UpdatedDate,
    string? OrderByColumn);

/// <summary>
/// Record for updating an existing report builder entry.
/// </summary>
public record UpdateReportBuilderRecord(
    int ReportHeaderId,
    string? ReportHeader,
    string? ReportTitle,
    int? QueryType,
    string? QueryText,
    int? IsActive,
    int? CreatedBy,
    DateTime? CreatedDate,
    int? UpdatedBy,
    DateTime? UpdatedDate,
    string? OrderByColumn);

/// <summary>
/// Record for deleting a report builder entry.
/// </summary>
public record DeleteReportBuilderRecord(int ReportHeaderId);
