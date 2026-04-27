namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM lead.</summary>
public record CreateCrmLeadRecord(
    string LeadName,
    string? Email,
    string? Phone,
    int? LeadSourceId,
    int? LeadStatusId,
    int? AssignedCounselorId,
    int? AgentId,
    string? CountryOfInterest,
    string? CourseOfInterest,
    decimal? Budget,
    string? Notes,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM lead.</summary>
public record UpdateCrmLeadRecord(
    int LeadId,
    string LeadName,
    string? Email,
    string? Phone,
    int? LeadSourceId,
    int? LeadStatusId,
    int? AssignedCounselorId,
    int? AgentId,
    string? CountryOfInterest,
    string? CourseOfInterest,
    decimal? Budget,
    string? Notes,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM lead.</summary>
/// <param name="LeadId">ID of the lead to delete.</param>
public record DeleteCrmLeadRecord(int LeadId);
