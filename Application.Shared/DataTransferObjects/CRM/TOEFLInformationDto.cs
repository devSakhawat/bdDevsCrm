using Microsoft.AspNetCore.Http;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class TOEFLInformationDto
{
  public int TOEFLInformationId { get; set; }
  public int ApplicantId { get; set; }
  public decimal? TOEFLListening { get; set; }
  public decimal? TOEFLReading { get; set; }
  public decimal? TOEFLWriting { get; set; }
  public decimal? TOEFLSpeaking { get; set; }
  public decimal? TOEFLOverallScore { get; set; }
  public DateTime? TOEFLDate { get; set; }

  public IFormFile? TOEFLScannedCopyFile { get; set; }
  public string? TOEFLScannedCopyFileName { get; set; }
  public string? TOEFLScannedCopyPath { get; set; }

  public string? TOEFLAdditionalInformation { get; set; }

  // Existing audit (save)
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }

  // Added to align with ApplicationDto aliases
  public DateTime? TOEFL_CreatedDate { get; set; }
  public int TOEFL_CreatedBy { get; set; }
  public DateTime? TOEFL_UpdatedDate { get; set; }
  public int? TOEFL_UpdatedBy { get; set; }
}

