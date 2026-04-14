namespace bdDevCRM.Shared.DataTransferObjects;

public class CrmCountryDto
{
  public int CountryId { get; set; }

  public string? CountryName { get; set; }

  public string? CountryCode { get; set; }

  public int? Status { get; set; }
}
