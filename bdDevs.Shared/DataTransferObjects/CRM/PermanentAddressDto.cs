using System;

namespace bdDevs.Shared.DataTransferObjects.CRM;

public class PermanentAddressDto
{
  public int PermanentAddressId { get; set; }
  public int ApplicantId { get; set; }

  // for save
  public string? Address { get; set; }
  public string? City { get; set; }
  public string? State { get; set; }
  public int CountryId { get; set; }
  public string? CountryName { get; set; }
  public string? PostalCode { get; set; }

  // Existing audit (save)
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }

  // Premanent Full Address
  public string PremanentFullAddress => $"{Address} {City} {CountryName} {PostalCode}".Trim();

  // for: get application info (aliases)
  public string? PermanentAddress { get; set; }
  public string? PermanentCity { get; set; }
  public string? PermanentState { get; set; }
  public int PermanentCountryId { get; set; }
  public string? PermanentCountryName { get; set; }
  public string? PermanentPostalCode { get; set; }

  // Added to align with ApplicationDto aliases
  public DateTime? PermanentCreatedDate { get; set; }
  public int PermanentCreatedBy { get; set; }
  public DateTime? PermanentUpdatedDate { get; set; }
  public int? PermanentUpdatedBy { get; set; }

  public string PremanentAddressFullAddress => $"{PermanentAddress} {PermanentCity} {PermanentState} {PermanentCountryName} {PermanentPostalCode}".Trim();
}