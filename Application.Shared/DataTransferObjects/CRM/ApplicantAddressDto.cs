namespace bdDevCRM.Shared.DataTransferObjects.CRM;

/// <summary>
/// Applicant Address DTO
/// </summary>
public class ApplicantAddressDto
{
  public PermanentAddressDto? PermanentAddress { get; set; }
  public PresentAddressDto? PresentAddress { get; set; }
}