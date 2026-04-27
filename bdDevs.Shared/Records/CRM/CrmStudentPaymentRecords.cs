namespace bdDevs.Shared.Records.CRM;

public record CreateCrmStudentPaymentRecord(int StudentId, int ApplicationId, int BranchId, byte PaymentType, decimal Amount, string Currency, decimal ExchangeRate, DateTime PaymentDate, string? PaymentMethod, string? BankName, string? TransactionRef, byte Status, int ReceivedBy, int? VerifiedBy, string? Notes, bool IsDeleted, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record UpdateCrmStudentPaymentRecord(int StudentPaymentId, int StudentId, int ApplicationId, int BranchId, byte PaymentType, string? ReceiptNo, decimal Amount, string Currency, decimal ExchangeRate, decimal AmountBdt, DateTime PaymentDate, string? PaymentMethod, string? BankName, string? TransactionRef, byte Status, int ReceivedBy, int? VerifiedBy, string? Notes, bool IsDeleted, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record DeleteCrmStudentPaymentRecord(int StudentPaymentId);
public record ChangeCrmStudentPaymentStatusRecord(int StudentPaymentId, byte NewStatus, int ChangedBy, string? Notes);
