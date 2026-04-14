using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class CrmApplicationGridDto
{
  // Hidden ID Fields
  public int RowIndex { get; set; }
  public int ApplicationId { get; set; }
  public int ApplicantId { get; set; }
  public int ApplicantCourseId { get; set; }
  public int CountryId { get; set; }
  public int InstituteId { get; set; }
  public int CourseId { get; set; }
  public int CurrencyId { get; set; }
  public int PaymentMethodId { get; set; }
  public int GenderId { get; set; }
  public int MaritalStatusId { get; set; }
  public int PermanentAddressId { get; set; }
  public int PresentAddressId { get; set; }

  // Basic Application Info
  public DateTime ApplicationDate { get; set; }
  public int StateId { get; set; }
  public string ApplicationStatus { get; set; } = null!;

  // Personal Details
  public string? TitleText { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string ApplicantName => $"{TitleText} {FirstName} {LastName}".Trim();

  public string? EmailAddress { get; set; }
  public string? Mobile { get; set; }
  public DateTime? DateOfBirth { get; set; }
  public string? GenderName { get; set; }
  public string? Nationality { get; set; }

  // Passport Information
  public bool? HasValidPassport { get; set; }
  public string? PassportNumber { get; set; }
  public DateTime? PassportExpiryDate { get; set; }

  // Course Information
  public string? CountryName { get; set; }
  public string? InstituteName { get; set; }
  public string? CourseTitle { get; set; }
  public string? IntakeMonth { get; set; }
  public string? IntakeYear { get; set; }

  // Financial Information
  public string? ApplicationFee { get; set; }
  public string? CurrencyName { get; set; }
  public string? PaymentMethod { get; set; }
  public DateTime? PaymentDate { get; set; }
  public string? PaymentReferenceNumber { get; set; }

  // Address Information
  public string? PermanentCountryName { get; set; }
  public string? PermanentCity { get; set; }
  public string? PresentCountryName { get; set; }
  public string? PresentCity { get; set; }

  // English Language Tests
  public string? IELTSOverallBand { get; set; }
  public string? TOEFLOverallScore { get; set; }
  public string? PTEOverallScore { get; set; }

  // Education Summary
  public string? HighestEducationLevel { get; set; }  
  public string? EducationGPA { get; set; }

  // Work Experience
  public string? TotalWorkExperience { get; set; }

  // Additional Information
  public bool HasStatementOfPurpose { get; set; }
  public int AdditionalDocumentsCount { get; set; }

  // File Indicators
  public string? ApplicantImagePath { get; set; }

  // Remarks
  public string? Remarks { get; set; }
}
