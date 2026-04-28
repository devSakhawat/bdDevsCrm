namespace bdDevs.Shared.Records.CRM;

public record CreateCrmPaymentRefundRecord(int PaymentId, decimal RefundAmount, DateTime RefundDate, string? RefundMethod, int? ApprovedBy, string? Reason, byte Status, DateTime? ProcessedDate, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record UpdateCrmPaymentRefundRecord(int PaymentRefundId, int PaymentId, decimal RefundAmount, DateTime RefundDate, string? RefundMethod, int? ApprovedBy, string? Reason, byte Status, DateTime? ProcessedDate, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record DeleteCrmPaymentRefundRecord(int PaymentRefundId);
