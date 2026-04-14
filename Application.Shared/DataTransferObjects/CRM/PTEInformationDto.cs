using Microsoft.AspNetCore.Http;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class PTEInformationDto
{
  public int PTEInformationId { get; set; }
  public int ApplicantId { get; set; }
  public decimal? PTEListening { get; set; }
  public decimal? PTEReading { get; set; }
  public decimal? PTEWriting { get; set; }
  public decimal? PTESpeaking { get; set; }
  public decimal? PTEOverallScore { get; set; }
  public DateTime? PTEDate { get; set; }

  public IFormFile? PTEScannedCopyFile { get; set; }
  public string? PTEScannedCopyFileName { get; set; }
  public string? PTEScannedCopyPath { get; set; }

  public string? PTEAdditionalInformation { get; set; }

  // Existing audit (save)
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }

  // Added to align with ApplicationDto aliases
  public DateTime PTE_CreatedDate { get; set; }
  public int PTE_CreatedBy { get; set; }
  public DateTime? PTE_UpdatedDate { get; set; }
  public int? PTE_UpdatedBy { get; set; }
}