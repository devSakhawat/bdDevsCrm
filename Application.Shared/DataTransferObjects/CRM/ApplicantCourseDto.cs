using System;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

/// <summary>
/// Applicant Course Form Details DTO
/// </summary>
public class ApplicantCourseDto
{
  public int ApplicantCourseId { get; set; }
  public int ApplicantId { get; set; }
  public int CountryId { get; set; }
  public string? CountryName { get; set; }
  public int InstituteId { get; set; }
  public string? InstituteName { get; set; }
  public string? CourseTitle { get; set; }
  public int IntakeMonthId { get; set; }
  public string? IntakeMonth { get; set; }
  public int IntakeYearId { get; set; }
  public string? IntakeYear { get; set; }

  // Align with ApplicationDto (decimal?)
  public decimal? ApplicationFee { get; set; }

  public int CurrencyId { get; set; }
  public string? CurrencyName { get; set; }
  public int PaymentMethodId { get; set; }
  public string? PaymentMethod { get; set; }
  public string? PaymentReferenceNumber { get; set; }
  public DateTime? PaymentDate { get; set; }

  public string? Remarks { get; set; }
  public string? CourseRemarks { get; set; }

  // Existing audit (save)
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }

  // Added to align with ApplicationDto aliases
  public DateTime? CourseCreatedDate { get; set; }
  public int CourseCreatedBy { get; set; }
  public DateTime? CourseUpdatedDate { get; set; }
  public int? CourseUpdatedBy { get; set; }

  public int? CourseId { get; set; }
}