namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM application.
/// </summary>
public record CreateCrmApplicationRecord(
    DateTime ApplicationDate,
    int StateId,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM application.
/// </summary>
public record UpdateCrmApplicationRecord(
    int ApplicationId,
    DateTime ApplicationDate,
    int StateId,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM application.
/// </summary>
/// <param name="ApplicationId">ID of the application to delete.</param>
public record DeleteCrmApplicationRecord(int ApplicationId);
