namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new access restriction.
/// </summary>
public record CreateAccessRestrictionRecord(
    int HrRecordId,
    int ReferenceId,
    int ReferenceType,
    DateTime? AccessDate,
    int? AccessBy,
    int? ParentReference,
    int? ChiledParentReference,
    int? RestrictionType,
    int? GroupId);

/// <summary>
/// Record for updating an existing access restriction.
/// </summary>
public record UpdateAccessRestrictionRecord(
    int AccessRestrictionId,
    int HrRecordId,
    int ReferenceId,
    int ReferenceType,
    DateTime? AccessDate,
    int? AccessBy,
    int? ParentReference,
    int? ChiledParentReference,
    int? RestrictionType,
    int? GroupId);

/// <summary>
/// Record for deleting an access restriction.
/// </summary>
public record DeleteAccessRestrictionRecord(int AccessRestrictionId);
