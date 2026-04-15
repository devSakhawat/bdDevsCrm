namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating new delegation info.
/// </summary>
public record CreateDelegationInfoRecord(
    int? HrRecordId,
    int? DeligatedHrRecordId,
    DateOnly? FromDate,
    DateOnly? ToDate,
    int? IsActive);

/// <summary>
/// Record for updating existing delegation info.
/// </summary>
public record UpdateDelegationInfoRecord(
    int DeligationId,
    int? HrRecordId,
    int? DeligatedHrRecordId,
    DateOnly? FromDate,
    DateOnly? ToDate,
    int? IsActive);

/// <summary>
/// Record for deleting delegation info.
/// </summary>
public record DeleteDelegationInfoRecord(int DeligationId);
