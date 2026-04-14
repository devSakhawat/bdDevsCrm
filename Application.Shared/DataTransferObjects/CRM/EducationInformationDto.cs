namespace bdDevCRM.Shared.DataTransferObjects.CRM;

/// <summary>
/// Education And English Language Form Section DTOs
/// </summary>
public class EducationInformationDto
{
  public EducationDetailsDto? EducationDetails { get; set; }
  public IELTSInformationDto? IELTSInformation { get; set; }
  public TOEFLInformationDto? TOEFLInformation { get; set; }
  public PTEInformationDto? PTEInformation { get; set; }
  public GMATInformationDto? GMATInformation { get; set; }
  public OthersInformationDto? OTHERSInformation { get; set; }
  public WorkExperienceDto? WorkExperience { get; set; }
}