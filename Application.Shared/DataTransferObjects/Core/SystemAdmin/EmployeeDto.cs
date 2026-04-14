namespace Application.Shared.DataTransferObjects.Core.SystemAdmin;


public class EmployeeDto
{
  public int HRRecordId { get; set; }
  public string FullName { get; set; }
  public string EmployeeId { get; set; }
  public string FatherName { get; set; }
  public string MotherName { get; set; }
  public string SpouseName { get; set; }
  public int Gender { get; set; }
  public string Religion { get; set; }

  public int ReligionId { get; set; }
  //  public string Nationality { get; set; }
  public int Nationality { get; set; }

  public string NationalID { get; set; }
  public string PassportNo { get; set; }
  public DateTime DateofBirth { get; set; }
  public string Dob { get; set; }

  //public string PlaceofBirth { get; set; }
  public int PlaceofBirth { get; set; }

  public DateTime DateofMarriage { get; set; }
  public string PresentAddress { get; set; }

  // public string Thana { get; set; }
  public int Thana { get; set; }

  //public string District { get; set; }
  public int District { get; set; }

  public string PermanentAddress { get; set; }

  // public string PermanentAddressThana { get; set; }
  public int PermanentAddressThana { get; set; }
  //public string PermanentAddressDistrict { get; set; }
  public int PermanentAddressDistrict { get; set; }

  public string HomePhone { get; set; }
  public string MobileNo { get; set; }
  public string PersonalEmail { get; set; }
  public string InternetMessenger { get; set; }
  public string InternetProfileLink { get; set; }
  public string AdditionalInfo { get; set; }
  public int AdditionalDayOf { get; set; }
  public int DayOfType { get; set; }
  public int AppliedDayOfWeek { get; set; }
  public int NumberofDays { get; set; }
  public int CasualLeaveNo { get; set; }
  public int MedicalLeaveNo { get; set; }
  public int AnualLeaveNo { get; set; }
  public int ShortLeaveNo { get; set; }
  public string BloodGroup { get; set; }
  public int StateId { get; set; }
  public string OriginalBirthDay { get; set; }
  public int UserId { get; set; }
  public DateTime LastUpdatedDate { get; set; }
  public int ApproverId { get; set; }
  public DateTime ApproveDate { get; set; }
  public bool LogHourEnable { get; set; }
  public int DepartmentId { get; set; }
  public string ProfilePicture { get; set; }
  public int Meritialstatus { get; set; }
  public string Birthidentification { get; set; }

  // public string Placeofpassportissue { get; set; }
  public int Placeofpassportissue { get; set; }

  public DateTime Passportissuedate { get; set; }
  public DateTime Passportexpiredate { get; set; }
  public string Height { get; set; }
  public string Weight { get; set; }
  public string Hobby { get; set; }
  public string Signature { get; set; }
  public string IdentificationMark { get; set; }

  public int CertificateTypeId { get; set; }
  public string CertificateTypeName { get; set; }
  public int CertificateTypee { get; set; }
  //public CertificateType CertificateType { get; set; }
  public int TAXEXAMPTION { get; set; }
  public int INVESTMENTAMOUNT { get; set; }

  //New added
  public string ShortName { get; set; }
  public int IsAutistic { get; set; }
  public string PresentPostCode { get; set; }
  public string PermanentPostCode { get; set; }

  public int EmployeeLevel { get; set; }
  public string Reference { get; set; }
  public int SubjectOfAccountId { get; set; }
  public int FunctionalRoleId { get; set; }
  public string CompanyName { get; set; }
}