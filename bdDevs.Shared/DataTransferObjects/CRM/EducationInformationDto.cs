namespace bdDevs.Shared.DataTransferObjects.CRM;

/// <summary>
/// Education And English Language Form Section DTOs
/// </summary>
public class EducationInformationDto
{
  public EducationDetailsDto? EducationDetails { get; set; }
  public IeltsInformationDto? IELTSInformation { get; set; }
  public ToeflInformationDto? TOEFLInformation { get; set; }
  public PteInformationDto? PTEInformation { get; set; }
  public GmatInformationDto? GMATInformation { get; set; }
  public OthersInformationDto? OTHERSInformation { get; set; }
  public WorkExperienceDto? WorkExperience { get; set; }
}