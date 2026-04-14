using Microsoft.AspNetCore.Http;
using System;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class EducationHistoryDto
{
  public int EducationHistoryId { get; set; }
  public int ApplicantId { get; set; }
  public string? Institution { get; set; }
  public string? Qualification { get; set; }
  public int? PassingYear { get; set; }
  public string? Grade { get; set; }

  public IFormFile? AttachedDocumentFile { get; set; }
  public string? DocumentName { get; set; }
  public string? DocumentPath { get; set; }
  public string? AttachedDocument { get; set; }
  public string? PdfThumbnail { get; set; }

  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }
}

