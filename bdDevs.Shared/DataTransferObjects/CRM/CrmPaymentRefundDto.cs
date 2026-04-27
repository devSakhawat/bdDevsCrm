namespace bdDevs.Shared.DataTransferObjects.CRM;

public class CrmPaymentRefundDto
{
    public int PaymentRefundId { get; set; }
    public int PaymentId { get; set; }
    public decimal RefundAmount { get; set; }
    public DateTime RefundDate { get; set; }
    public string? RefundMethod { get; set; }
    public int? ApprovedBy { get; set; }
    public string? Reason { get; set; }
    public byte Status { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
