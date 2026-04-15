using System;

namespace bdDevs.Shared.DataTransferObjects.CRM;

/// <summary>
/// Complete CRM Application Data Transfer Object
/// </summary>
public class CrmApplicationDto
{
  public int ApplicationId { get; set; }

  public DateTime? ApplicationDate { get; set; }

  public int StateId { get; set; }


  // Existing audit (save)
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }

  // Added to align with ApplicationDto (for read)
  public DateTime? AppCreatedDate { get; set; }
  public int AppCreatedBy { get; set; }
  public DateTime? AppUpdatedDate { get; set; }
  public int? AppUpdatedBy { get; set; }

  public CourseInformationDto? CourseInformation { get; set; }
  public EducationInformationDto? EducationInformation { get; set; }
  public AdditionalInformationDto? AdditionalInformation { get; set; }

  public CrmApplicationDto()
  {
    CourseInformation = new CourseInformationDto
    {
      ApplicantAddress = new ApplicantAddressDto
      {
        PermanentAddress = new PermanentAddressDto(),
        PresentAddress = new PresentAddressDto()
      },
      ApplicantCourse = new ApplicantCourseDto(),
      PersonalDetails = new ApplicantInfoDto()
    };

    EducationInformation = new EducationInformationDto
    {
      EducationDetails = new EducationDetailsDto(),
      IELTSInformation = new IeltsInformationDto(),
      TOEFLInformation = new ToeflInformationDto(),
      PTEInformation = new PteInformationDto(),
      GMATInformation = new GmatInformationDto(),
      OTHERSInformation = new OthersInformationDto(),
      WorkExperience = new WorkExperienceDto()
    };

    // Pre-initialize list properties to avoid null errors
    EducationInformation.EducationDetails.EducationHistory = new List<EducationHistoryDto>();
    EducationInformation.WorkExperience.WorkExperienceHistory = new List<WorkExperienceHistoryDto>();

    AdditionalInformation = new AdditionalInformationDto
    {
      ReferenceDetails = new ReferenceDetailsDto { References = new List<ApplicantReferenceDto>() },
      StatementOfPurpose = new StatementOfPurposeDto(),
      AdditionalInformation = new AdditionalInfoDto(),
      AdditionalDocuments = new AdditionalDocumentsDto { Documents = new List<AdditionalDocumentDto>() }
    };

    // Pre-initialize list properties to avoid null errors
    AdditionalInformation.ReferenceDetails.References = new List<ApplicantReferenceDto>();
    AdditionalInformation.AdditionalDocuments.Documents = new List<AdditionalDocumentDto>();
  }

}