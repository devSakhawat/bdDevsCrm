namespace bdDevs.Shared.Records.Core.SystemAdmin;

public record CreateCurrencyRateRecord(
    int CurrencyId,
    decimal? CurrencyRateRation,
    DateTime? CurrencyMonth,
    int? CreatedBy);

public record UpdateCurrencyRateRecord(
    int CurencyRateId,
    int CurrencyId,
    decimal? CurrencyRateRation,
    DateTime? CurrencyMonth);

public record DeleteCurrencyRateRecord(int CurencyRateId);
