using Microsoft.AspNetCore.Http;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class IELTSInformationDto
{
  public int IELTSInformationId { get; set; }
  public int ApplicantId { get; set; }
  public decimal? IELTSListening { get; set; }
  public decimal? IELTSReading { get; set; }
  public decimal? IELTSWriting { get; set; }
  public decimal? IELTSSpeaking { get; set; }
  public decimal? IELTSOverallScore { get; set; }
  public DateTime? IELTSDate { get; set; }

  public IFormFile? IELTSScannedCopyFile { get; set; }
  public string? IELTSScannedCopyFileName { get; set; }
  public string? IELTSScannedCopyPath { get; set; }

  public string? IELTSAdditionalInformation { get; set; }

  // Existing audit (save)
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }

  // Added to align with ApplicationDto aliases
  public DateTime? IELTS_CreatedDate { get; set; }
  public int IELTS_CreatedBy { get; set; }
  public DateTime? IELTS_UpdatedDate { get; set; }
  public int? IELTS_UpdatedBy { get; set; }
}