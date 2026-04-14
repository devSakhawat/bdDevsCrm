using System;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class ApplicantReferenceDto
{
  public int ApplicantReferenceId { get; set; }

  public int ApplicantId { get; set; }

  public string Name { get; set; } = null!;

  public string? Designation { get; set; }

  public string? Institution { get; set; }

  public string? EmailID { get; set; }

  public string? PhoneNo { get; set; }

  public string? FaxNo { get; set; }

  public string? Address { get; set; }

  public string? City { get; set; }

  public string? State { get; set; }

  public string? Country { get; set; }

  public string? PostOrZipCode { get; set; }

  public DateTime CreatedDate { get; set; }

  public int CreatedBy { get; set; }

  public DateTime? UpdatedDate { get; set; }

  public int? UpdatedBy { get; set; }
}