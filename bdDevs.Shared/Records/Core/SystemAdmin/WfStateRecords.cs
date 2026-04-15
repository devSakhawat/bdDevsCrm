namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new workflow state.
/// </summary>
public record CreateWfStateRecord(
    string StateName,
    int MenuId,
    bool? IsDefaultStart,
    int? IsClosed,
    int? Sequence);

/// <summary>
/// Record for updating an existing workflow state.
/// </summary>
public record UpdateWfStateRecord(
    int WfStateId,
    string StateName,
    int MenuId,
    bool? IsDefaultStart,
    int? IsClosed,
    int? Sequence);

/// <summary>
/// Record for deleting a workflow state.
/// </summary>
public record DeleteWfStateRecord(int WfStateId);
