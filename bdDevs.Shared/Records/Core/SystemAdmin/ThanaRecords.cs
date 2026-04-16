namespace bdDevs.Shared.Records.Core.SystemAdmin;

public record CreateThanaRecord(
    int DistrictId,
    string? ThanaName,
    string? ThanaCode,
    int? Status,
    string? ThanaNameBn);

public record UpdateThanaRecord(
    int ThanaId,
    int DistrictId,
    string? ThanaName,
    string? ThanaCode,
    int? Status,
    string? ThanaNameBn);

public record DeleteThanaRecord(int ThanaId);
