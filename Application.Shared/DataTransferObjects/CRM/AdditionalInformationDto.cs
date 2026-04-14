namespace bdDevCRM.Shared.DataTransferObjects.CRM;

/// <summary>
/// Additional Information Section DTOs
/// </summary>
public class AdditionalInformationDto
{
  public ReferenceDetailsDto? ReferenceDetails { get; set; }
  public StatementOfPurposeDto? StatementOfPurpose { get; set; }
  public AdditionalInfoDto? AdditionalInformation { get; set; }
  public AdditionalDocumentsDto? AdditionalDocuments { get; set; }
}