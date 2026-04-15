namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating new document details history.
/// </summary>
public record CreateDocmdetailshistoryRecord(
    int DocumentId,
    int UploadedBy,
    int DepartmentId,
    int Responsiblepersonto,
    string? Subject,
    string? Filename,
    string? Filedescription,
    string? Fullpath,
    int? Status,
    DateTime? UploadedDate,
    int? Lastopenorclosebyid,
    DateTime? Lastupdate,
    string? Remarks);

/// <summary>
/// Record for updating existing document details history.
/// </summary>
public record UpdateDocmdetailshistoryRecord(
    int DocumentHistoryId,
    int DocumentId,
    int UploadedBy,
    int DepartmentId,
    int Responsiblepersonto,
    string? Subject,
    string? Filename,
    string? Filedescription,
    string? Fullpath,
    int? Status,
    DateTime? UploadedDate,
    int? Lastopenorclosebyid,
    DateTime? Lastupdate,
    string? Remarks);

/// <summary>
/// Record for deleting document details history.
/// </summary>
public record DeleteDocmdetailshistoryRecord(int DocumentHistoryId);
