using System;

namespace bdDevs.Shared.DataTransferObjects.CRM;

public class PresentAddressDto
{
  public int PresentAddressId { get; set; }
  public int ApplicantId { get; set; }
  public bool SameAsPermanentAddress { get; set; }

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

  // Permanent Full Address
  public string PermanentFullAddress => $"{Address} {City} {CountryName} {PostalCode}".Trim();

  // for: get application info (aliases)
  public string? PresentAddress { get; set; }
  public string? PresentCity { get; set; }
  public string? PresentState { get; set; }
  public int PresentCountryId { get; set; }
  public string? PresentCountryName { get; set; }

  // Fix typo and align with ApplicationDto
  public string? PresentPostalCode { get; set; }

  // Keep old typo if referenced elsewhere
  public string? PresenPostalCode { get; set; }

  // Added to align with ApplicationDto aliases
  public DateTime? PresentCreatedDate { get; set; }
  public int PresentCreatedBy { get; set; }
  public DateTime? PresentUpdatedDate { get; set; }
  public int? PresentUpdatedBy { get; set; }

  public string PermanentAddressFullAddress => $"{PresentAddress} {PresentCity} {PresentState} {PresentCountryName} {PresentPostalCode}".Trim();
}