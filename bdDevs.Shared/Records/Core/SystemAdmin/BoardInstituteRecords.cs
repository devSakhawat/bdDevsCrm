namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new board/institute.
/// </summary>
public record CreateBoardInstituteRecord(
    string? BoardInstituteName,
    int? IsActive);

/// <summary>
/// Record for updating an existing board/institute.
/// </summary>
public record UpdateBoardInstituteRecord(
    int BoardInstituteId,
    string? BoardInstituteName,
    int? IsActive);

/// <summary>
/// Record for deleting a board/institute.
/// </summary>
public record DeleteBoardInstituteRecord(int BoardInstituteId);
