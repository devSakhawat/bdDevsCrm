namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM institute type.
/// </summary>
public record CreateCrmInstituteTypeRecord(
    string InstituteTypeName);

/// <summary>
/// Record for updating an existing CRM institute type.
/// </summary>
public record UpdateCrmInstituteTypeRecord(
    int InstituteTypeId,
    string InstituteTypeName);

/// <summary>
/// Record for deleting a CRM institute type.
/// </summary>
/// <param name="InstituteTypeId">ID of the institute type to delete.</param>
public record DeleteCrmInstituteTypeRecord(int InstituteTypeId);
