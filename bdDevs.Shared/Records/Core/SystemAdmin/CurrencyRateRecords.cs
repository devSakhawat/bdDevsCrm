namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new currency rate.
/// </summary>
public record CreateCurrencyRateRecord(
    int CurrencyId,
    decimal? CurrencyRateRation,
    DateTime? CurrencyMonth);

/// <summary>
/// Record for updating an existing currency rate.
/// </summary>
public record UpdateCurrencyRateRecord(
    int CurencyRateId,
    int CurrencyId,
    decimal? CurrencyRateRation,
    DateTime? CurrencyMonth);

/// <summary>
/// Record for deleting a currency rate.
/// </summary>
public record DeleteCurrencyRateRecord(int CurencyRateId);
