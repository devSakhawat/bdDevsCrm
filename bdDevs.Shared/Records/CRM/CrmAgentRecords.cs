namespace bdDevs.Shared.Records.CRM;

public record CreateCrmAgentRecord(
    string AgentName,
    string? AgencyName,
    string PrimaryPhone,
    string? PrimaryEmail,
    int CommissionTypeId,
    decimal? DefaultCommissionValue,
    int CountryId,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record UpdateCrmAgentRecord(
    int AgentId,
    string AgentName,
    string? AgencyName,
    string PrimaryPhone,
    string? PrimaryEmail,
    int CommissionTypeId,
    decimal? DefaultCommissionValue,
    int CountryId,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record DeleteCrmAgentRecord(int AgentId);
