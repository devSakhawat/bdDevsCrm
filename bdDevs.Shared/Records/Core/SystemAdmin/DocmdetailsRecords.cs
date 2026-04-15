namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating new document details.
/// </summary>
public record CreateDocmdetailsRecord(
    int UploadedBy,
    int DepartmentId,
    int Responsiblepersonto,
    string? Subject,
    string? Filename,
    string? Filedescription,
    string? Fullpath,
    int? StatusId,
    DateTime? UploadedDate,
    int? Lastopenorclosebyid,
    DateTime? Lastupdate,
    string? Remarks);

/// <summary>
/// Record for updating existing document details.
/// </summary>
public record UpdateDocmdetailsRecord(
    int DocumentId,
    int UploadedBy,
    int DepartmentId,
    int Responsiblepersonto,
    string? Subject,
    string? Filename,
    string? Filedescription,
    string? Fullpath,
    int? StatusId,
    DateTime? UploadedDate,
    int? Lastopenorclosebyid,
    DateTime? Lastupdate,
    string? Remarks);

/// <summary>
/// Record for deleting document details.
/// </summary>
public record DeleteDocmdetailsRecord(int DocumentId);
