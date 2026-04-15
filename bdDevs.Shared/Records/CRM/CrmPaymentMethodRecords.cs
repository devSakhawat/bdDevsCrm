namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM payment method.
/// </summary>
public record CreateCrmPaymentMethodRecord(
    string PaymentMethodName,
    string? PaymentMethodCode,
    string? Description,
    decimal? ProcessingFee,
    string? ProcessingFeeType,
    bool IsOnlinePayment,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM payment method.
/// </summary>
public record UpdateCrmPaymentMethodRecord(
    int PaymentMethodId,
    string PaymentMethodName,
    string? PaymentMethodCode,
    string? Description,
    decimal? ProcessingFee,
    string? ProcessingFeeType,
    bool IsOnlinePayment,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM payment method.
/// </summary>
/// <param name="PaymentMethodId">ID of the payment method to delete.</param>
public record DeleteCrmPaymentMethodRecord(int PaymentMethodId);
