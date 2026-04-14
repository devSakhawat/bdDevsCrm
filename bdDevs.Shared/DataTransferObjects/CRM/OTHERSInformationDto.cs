using Microsoft.AspNetCore.Http;
using System;

namespace bdDevs.Shared.DataTransferObjects.CRM;

public class OthersInformationDto
{
  // Existing field (save)
  public int OthersInformationId { get; set; }

  // Added alias to align with ApplicationDto
  //public int OTHERSInformationId { get; set; }

  public int ApplicantId { get; set; }

  public string? AdditionalInformation { get; set; }

  public IFormFile? OTHERSScannedCopyFile { get; set; }
  public string? OTHERSScannedCopyFileName { get; set; }
  public string? OTHERSScannedCopyPath { get; set; }

  // Existing audit (save)
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }

  // Added to align with ApplicationDto aliases
  public DateTime OTHERS_CreatedDate { get; set; }
  public int OTHERS_CreatedBy { get; set; }
  public DateTime? OTHERS_UpdatedDate { get; set; }
  public int? OTHERS_UpdatedBy { get; set; }
}