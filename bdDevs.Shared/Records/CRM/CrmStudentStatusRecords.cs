namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM student status.</summary>
public record CreateCrmStudentStatusRecord(
    string StatusName,
    string? StatusCode,
    string? ColorCode,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM student status.</summary>
public record UpdateCrmStudentStatusRecord(
    int StudentStatusId,
    string StatusName,
    string? StatusCode,
    string? ColorCode,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM student status.</summary>
/// <param name="StudentStatusId">ID of the student status to delete.</param>
public record DeleteCrmStudentStatusRecord(int StudentStatusId);
