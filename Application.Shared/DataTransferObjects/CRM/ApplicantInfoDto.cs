//using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Http;

namespace bdDevs.Shared.DataTransferObjects.CRM;

/// <summary>
/// Personal Details DTO
/// </summary>
public class ApplicantInfoDto
{
  public int ApplicantId { get; set; }
  public int ApplicationId { get; set; }
  public int GenderId { get; set; }
  public string? GenderName { get; set; }
  public string? TitleValue { get; set; }
  public string? TitleText { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }

  // New: align with ApplicationDto
  public string? ApplicantName { get; set; }

  // Computed
  public string FullName => $"{TitleText} {FirstName} {LastName}".Trim();

  public DateTime? DateOfBirth { get; set; }
  public int MaritalStatusId { get; set; }
  public string? MaritalStatusName { get; set; }
  public string? Nationality { get; set; }
  public bool? HasValidPassport { get; set; }
  public string? PassportNumber { get; set; }
  public DateTime? PassportIssueDate { get; set; }
  public DateTime? PassportExpiryDate { get; set; }
  public string? PhoneCountryCode { get; set; }
  public string? PhoneAreaCode { get; set; }
  public string? PhoneNumber { get; set; }
  public string? Mobile { get; set; }
  public string? EmailAddress { get; set; }
  public string? SkypeId { get; set; }

  // File Handle
  public IFormFile? ApplicantImageFile { get; set; }
  public string? ApplicantImagePath { get; set; }
  public string? ApplicantImagePreview { get; set; }

  // Existing audit (save)
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }

  // Added to align with ApplicationDto aliases
  public DateTime? ApplicantCreatedDate { get; set; }
  public int ApplicantCreatedBy { get; set; }
  public DateTime? ApplicantUpdatedDate { get; set; }
  public int? ApplicantUpdatedBy { get; set; }
}