namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM lead source.</summary>
public record CreateCrmLeadSourceRecord(
    string SourceName,
    string? SourceCode,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM lead source.</summary>
public record UpdateCrmLeadSourceRecord(
    int LeadSourceId,
    string SourceName,
    string? SourceCode,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM lead source.</summary>
/// <param name="LeadSourceId">ID of the lead source to delete.</param>
public record DeleteCrmLeadSourceRecord(int LeadSourceId);
