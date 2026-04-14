using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevs.Shared.DataTransferObjects.Core.HR;

public class EmploymentDto
{
  public int RequisitionId { get; set; }
  public int HRRecordId { get; set; }
  public string EmployeeId { get; set; }
  public int EmployeeType { get; set; }
  public string CurrentPosition { get; set; }
  public DateTime StartDate { get; set; }
  public DateTime EmploymentDate { get; set; }
  public int CompanyId { get; set; }
  public int DepartmentId { get; set; }
  public int ReportTo { get; set; }
  public int Approver { get; set; }
  public string TelephoneExtension { get; set; }
  public string OfficialEmail { get; set; }
  public string EmergencyContactName { get; set; }
  public string EmergencyContactNo { get; set; }
  public string Duties { get; set; }
  public string AttendanceCardNo { get; set; }
  public int UserId { get; set; }
  public DateTime LastUpdatedDate { get; set; }
  public int BankBranchId { get; set; }
  public string BankAccountNo { get; set; }
  public int BranchId { get; set; }
  public int ShiftId { get; set; }
  public int DesignationId { get; set; }
  public int OldDesignationId { get; set; }
  public string Gpfno { get; set; }
  //public DateTime Retairementdate { get; set; }
  public DateTime JobEndDate { get; set; }
  public int JobEndTypeId { get; set; }
  public string JobEndTypeName { get; set; }
  public int Joiningpost { get; set; }
  public string Experience { get; set; }
  public int ReportingDepartmentId { get; set; }
  public int ApproverDepartmentId { get; set; }

  public DateTime ContractEndDate { get; set; }
  public int Func_Id { get; set; }
  public int GradeId { get; set; }
  public DateTime EffectEndDate { get; set; }
  public string TinNumber { get; set; }
  public int PostingType { get; set; }

  public int IsOTEligible { get; set; }
  public int NightShift { get; set; }

  public string ContactAddress { get; set; }

  public int IsFieldForce { get; set; }

  public int DivisionId { get; set; }
  public int FacilityId { get; set; }
  public int SectionId { get; set; }
  public int SubSectionId { get; set; }

  public int IsReserved { get; set; }
  public DateTime AppointmentDate { get; set; }

  public int SalaryLocation { get; set; }

  public DateTime ConfirmationDate { get; set; }
  public DateTime PossibleConfirmationDate { get; set; }

  public int OmitLate { get; set; }
  public int RosterFwdEmpId { get; set; }
  public string JobResponsibilities { get; set; }
  public string FunctionalJob { get; set; }
  public int ApplicantId { get; set; }
  public string SeparationRemarks { get; set; }
  public DateTime ResignationSummiteDate { get; set; }

  public int IsTemporary { get; set; }
  public int CostcentreId { get; set; }

  public int IsContract { get; set; }
  public int IsProbationary { get; set; }
  public int JobId { get; set; }
  public int RequisitionReferenceId { get; set; }
  public int IsProbExtnAllow { get; set; }
  public int VerificationStatus { get; set; }
  public string SIGNATURE { get; set; }
  public string FullName { get; set; }
  public int EmployeeLevel { get; set; }

}

