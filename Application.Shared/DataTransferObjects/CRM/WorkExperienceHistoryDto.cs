using Microsoft.AspNetCore.Http;
using System;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class WorkExperienceHistoryDto
{
  public int WorkExperienceId { get; set; }
  public int ApplicantId { get; set; } // Foreign Key
  public string? NameOfEmployer { get; set; }
  public string? Position { get; set; }
  public DateTime? StartDate { get; set; }
  public DateTime? EndDate { get; set; }
  public string? Period { get; set; }
  public string? MainResponsibility { get; set; }


  public IFormFile? ScannedCopyFile { get; set; }
  public string? ScannedCopyFileName { get; set; }
  public string? ScannedCopyPath { get; set; }
  //public string? ScannedCopy { get; set; }
  public string? DocumentName { get; set; }
  public string? FileThumbnail { get; set; }


  
  // Common fields
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }
}