namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new currency.
/// </summary>
/// <param name="CurrencyName">Name of the currency.</param>
/// <param name="CurrencyCode">ISO currency code.</param>
public record CreateCurrencyRecord(
    string CurrencyName,
    string CurrencyCode);

/// <summary>
/// Record for updating an existing currency.
/// </summary>
/// <param name="CurrencyId">ID of the currency to update.</param>
/// <param name="CurrencyName">Updated currency name.</param>
/// <param name="CurrencyCode">Updated ISO currency code.</param>
public record UpdateCurrencyRecord(
    int CurrencyId,
    string CurrencyName,
    string CurrencyCode);

/// <summary>
/// Record for deleting a currency.
/// </summary>
/// <param name="CurrencyId">ID of the currency to delete.</param>
public record DeleteCurrencyRecord(int CurrencyId);
