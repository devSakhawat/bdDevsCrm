namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class CurrencyRateDto
{
    public int CurencyRateId { get; set; }
    public int CurrencyId { get; set; }
    public decimal? CurrencyRateRation { get; set; }
    public DateTime? CurrencyMonth { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public class CurrencyRateDDLDto
{
    public int CurencyRateId { get; set; }
    public decimal? CurrencyRateRation { get; set; }
    public DateTime? CurrencyMonth { get; set; }
}
