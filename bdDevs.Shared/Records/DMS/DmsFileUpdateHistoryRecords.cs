namespace bdDevs.Shared.Records.DMS;

public record CreateDmsFileUpdateHistoryRecord(
    string? EntityId,
    string? EntityType,
    string? DocumentType,
    string? OldFilePath,
    string? NewFilePath,
    int? VersionNumber,
    string? UpdatedBy,
    string? UpdateReason,
    string? Notes);

public record UpdateDmsFileUpdateHistoryRecord(
    int Id,
    string? EntityId,
    string? EntityType,
    string? DocumentType,
    string? OldFilePath,
    string? NewFilePath,
    int? VersionNumber,
    string? UpdatedBy,
    string? UpdateReason,
    string? Notes);

public record DeleteDmsFileUpdateHistoryRecord(int Id);
