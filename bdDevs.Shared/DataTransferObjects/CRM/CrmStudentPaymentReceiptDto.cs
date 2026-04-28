namespace bdDevs.Shared.DataTransferObjects.CRM;

public class CrmStudentPaymentReceiptDto
{
    public int StudentPaymentId { get; set; }
    public string ReceiptNo { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public int ApplicationId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal AmountBdt { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? BankName { get; set; }
    public string? TransactionRef { get; set; }
    public byte Status { get; set; }
}
