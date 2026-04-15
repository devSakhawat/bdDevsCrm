using Microsoft.AspNetCore.Http;

namespace bdDevs.Shared.DataTransferObjects.CRM;

public class GmatInformationDto
{
  public int GMATInformationId { get; set; }
  public int ApplicantId { get; set; }
  public decimal? GMATListening { get; set; }
  public decimal? GMATReading { get; set; }
  public decimal? GMATWriting { get; set; }
  public decimal? GMATSpeaking { get; set; }
  public decimal? GMATOverallScore { get; set; }
  public DateTime? GMATDate { get; set; }

  public IFormFile? GMATScannedCopyFile { get; set; }
  public string? GMATScannedCopyFileName { get; set; }
  public string? GMATScannedCopyPath { get; set; }

  public string? GMATAdditionalInformation { get; set; }

  // Existing audit (save)
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }

  // Added to align with ApplicationDto aliases
  public DateTime? GMAT_CreatedDate { get; set; }
  public int GMAT_CreatedBy { get; set; }
  public DateTime? GMAT_UpdatedDate { get; set; }
  public int? GMAT_UpdatedBy { get; set; }
}