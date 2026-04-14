namespace bdDevs.Shared.DataTransferObjects.Core.HR;

public class EmployeesByCompanyBranchDepartmentDto
{
  public int HrRecordId { get; set; }
  public string FullName { get; set; }
  public string EmployeeId { get; set; }
  public int CompanyId { get; set; }
  // Report to EmployeeId
  public string ReportTo { get; set; }
  public int DepartmentId { get; set; }
  public int BranchId { get; set; }
}
