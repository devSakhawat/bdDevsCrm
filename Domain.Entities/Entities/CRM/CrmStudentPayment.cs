namespace Domain.Entities.Entities.CRM;

public partial class CrmStudentPayment
{
    public int StudentPaymentId { get; set; }
    public int StudentId { get; set; }
    public int ApplicationId { get; set; }
    public int BranchId { get; set; }
    public byte PaymentType { get; set; }
    public string? ReceiptNo { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "BDT";
    public decimal ExchangeRate { get; set; }
    public decimal AmountBdt { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? BankName { get; set; }
    public string? TransactionRef { get; set; }
    public byte Status { get; set; }
    public int ReceivedBy { get; set; }
    public int? VerifiedBy { get; set; }
    public string? Notes { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
