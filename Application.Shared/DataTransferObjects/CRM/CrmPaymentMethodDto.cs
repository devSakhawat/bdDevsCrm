namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public record CrmPaymentMethodDto
{
  public int PaymentMethodId { get; init; }
  public string PaymentMethodName { get; init; } = string.Empty;
  public string? PaymentMethodCode { get; init; }
  public string? Description { get; init; }
  public decimal? ProcessingFee { get; init; }
  public string? ProcessingFeeType { get; init; }
  public bool IsOnlinePayment { get; init; }
  public bool IsActive { get; init; }
  public DateTime CreatedDate { get; init; }
  public int CreatedBy { get; init; }
  public DateTime? UpdatedDate { get; init; }
  public int? UpdatedBy { get; init; }
}

public record CrmPaymentMethodDDL
{
  public int PaymentMethodId { get; init; }
  public string PaymentMethodName { get; init; } = string.Empty;
  public string? PaymentMethodCode { get; init; }
  public bool IsOnlinePayment { get; init; }
  public decimal? ProcessingFee { get; init; }
  public string? ProcessingFeeType { get; init; }
}