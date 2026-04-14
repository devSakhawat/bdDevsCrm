namespace bdDevs.Shared.DataTransferObjects.CRM;

public class AdditionalInfoDto
{
  public int AdditionalInfoId { get; set; }

  // Existing property (save)
  public int ApplicantId { get; set; }

  // Added alias to align with ApplicationDto
  public int AddInfo_ApplicantId { get; set; }

  // Align types with entity/DTO (bool?)
  public bool? RequireAccommodation { get; set; }
  public bool? HealthNMedicalNeeds { get; set; }

  public string? HealthNMedicalNeedsRemarks { get; set; }
  public string? AdditionalInformationRemarks { get; set; }

  // Existing audit (save)
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }

  // Added to align with ApplicationDto aliases
  public DateTime? AddInfoCreatedDate { get; set; }
  public int AddInfoCreatedBy { get; set; }
  public DateTime? AddInfoUpdatedDate { get; set; }
  public int? AddInfoUpdatedBy { get; set; }
}