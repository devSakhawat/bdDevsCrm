namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM agent type.</summary>
public record CreateCrmAgentTypeRecord(
    string AgentTypeName,
    string? Description,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM agent type.</summary>
public record UpdateCrmAgentTypeRecord(
    int AgentTypeId,
    string AgentTypeName,
    string? Description,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM agent type.</summary>
/// <param name="AgentTypeId">ID of the agent type to delete.</param>
public record DeleteCrmAgentTypeRecord(int AgentTypeId);
