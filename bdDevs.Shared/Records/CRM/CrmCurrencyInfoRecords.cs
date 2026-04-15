namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM currency info.
/// </summary>
public record CreateCrmCurrencyInfoRecord(
    string? CurrencyName,
    int? IsDefault,
    int? IsActive,
    int? CreatedBy,
    DateTime? CreatedDate,
    int? UpdatedBy,
    DateTime? UpdatedDate);

/// <summary>
/// Record for updating an existing CRM currency info.
/// </summary>
public record UpdateCrmCurrencyInfoRecord(
    int CurrencyId,
    string? CurrencyName,
    int? IsDefault,
    int? IsActive,
    int? CreatedBy,
    DateTime? CreatedDate,
    int? UpdatedBy,
    DateTime? UpdatedDate);

/// <summary>
/// Record for deleting a CRM currency info.
/// </summary>
/// <param name="CurrencyId">ID of the currency to delete.</param>
public record DeleteCrmCurrencyInfoRecord(int CurrencyId);
