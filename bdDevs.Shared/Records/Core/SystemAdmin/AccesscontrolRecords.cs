namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new access control.
/// </summary>
public record CreateAccesscontrolRecord(
    string AccessName);

/// <summary>
/// Record for updating an existing access control.
/// </summary>
public record UpdateAccesscontrolRecord(
    int AccessId,
    string AccessName);

/// <summary>
/// Record for deleting an access control.
/// </summary>
public record DeleteAccesscontrolRecord(int AccessId);
