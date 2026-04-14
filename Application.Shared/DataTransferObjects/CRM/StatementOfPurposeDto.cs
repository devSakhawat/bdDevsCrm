using Microsoft.AspNetCore.Http;
using System;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class StatementOfPurposeDto
{
  public int StatementOfPurposeId { get; set; }
  public int ApplicantId { get; set; } // Foreign Key
  public string? StatementOfPurposeRemarks { get; set; }

  public string? StatementOfPurposeFileName { get; set; }
  public string? StatementOfPurposeFilePath { get; set; }
  public IFormFile? StatementOfPurposeFile { get; set; }

  // Common fields
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }
}