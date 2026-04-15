namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM statement of purpose.
/// </summary>
public record CreateCrmStatementOfPurposeRecord(
    int ApplicantId,
    string? StatementOfPurposeRemarks,
    string? StatementOfPurposeFilePath,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM statement of purpose.
/// </summary>
public record UpdateCrmStatementOfPurposeRecord(
    int StatementOfPurposeId,
    int ApplicantId,
    string? StatementOfPurposeRemarks,
    string? StatementOfPurposeFilePath,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM statement of purpose.
/// </summary>
/// <param name="StatementOfPurposeId">ID of the statement of purpose to delete.</param>
public record DeleteCrmStatementOfPurposeRecord(int StatementOfPurposeId);
