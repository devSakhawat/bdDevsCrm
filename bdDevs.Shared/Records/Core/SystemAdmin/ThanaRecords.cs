namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new thana.
/// </summary>
public record CreateThanaRecord(
    int DistrictId,
    string? ThanaName,
    string? ThanaCode,
    int? Status,
    string? ThanaNameBn);

/// <summary>
/// Record for updating an existing thana.
/// </summary>
public record UpdateThanaRecord(
    int ThanaId,
    int DistrictId,
    string? ThanaName,
    string? ThanaCode,
    int? Status,
    string? ThanaNameBn);

/// <summary>
/// Record for deleting a thana.
/// </summary>
public record DeleteThanaRecord(int ThanaId);
