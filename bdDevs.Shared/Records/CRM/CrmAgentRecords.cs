namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM agent.</summary>
public record CreateCrmAgentRecord(
    string AgentName,
    string? AgentCode,
    int AgentTypeId,
    string? ContactPerson,
    string? Email,
    string? Phone,
    string? Address,
    string? Country,
    decimal? CommissionRate,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM agent.</summary>
public record UpdateCrmAgentRecord(
    int AgentId,
    string AgentName,
    string? AgentCode,
    int AgentTypeId,
    string? ContactPerson,
    string? Email,
    string? Phone,
    string? Address,
    string? Country,
    decimal? CommissionRate,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM agent.</summary>
/// <param name="AgentId">ID of the agent to delete.</param>
public record DeleteCrmAgentRecord(int AgentId);
