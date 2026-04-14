namespace bdDevCRM.Shared.DataTransferObjects.CRM;

/// <summary>
/// Course Information Section DTO
/// </summary>
public class CourseInformationDto
{
  public ApplicantCourseDto? ApplicantCourse { get; set; }
  public ApplicantInfoDto? PersonalDetails { get; set; }
  public ApplicantAddressDto? ApplicantAddress { get; set; }
}