namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM lead status.</summary>
public record CreateCrmLeadStatusRecord(
    string StatusName,
    string? StatusCode,
    string? ColorCode,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM lead status.</summary>
public record UpdateCrmLeadStatusRecord(
    int LeadStatusId,
    string StatusName,
    string? StatusCode,
    string? ColorCode,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM lead status.</summary>
/// <param name="LeadStatusId">ID of the lead status to delete.</param>
public record DeleteCrmLeadStatusRecord(int LeadStatusId);
