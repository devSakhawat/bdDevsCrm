namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new workflow action.
/// </summary>
public record CreateWfActionRecord(
    int WfStateId,
    string ActionName,
    int NextStateId,
    int? EmailAlert,
    int? SmsAlert,
    int? AcSortOrder);

/// <summary>
/// Record for updating an existing workflow action.
/// </summary>
public record UpdateWfActionRecord(
    int WfActionId,
    int WfStateId,
    string ActionName,
    int NextStateId,
    int? EmailAlert,
    int? SmsAlert,
    int? AcSortOrder);

/// <summary>
/// Record for deleting a workflow action.
/// </summary>
public record DeleteWfActionRecord(int WfActionId);
