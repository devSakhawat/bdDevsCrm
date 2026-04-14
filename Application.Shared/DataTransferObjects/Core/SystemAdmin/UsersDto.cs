namespace Application.Shared.DataTransferObjects.Core.SystemAdmin;

public class UsersDto
{
  public int? UserId { get; set; }
  public int? HrRecordId { get; set; }
	public int? CompanyId { get; set; }
	public string? LoginId { get; set; }
	public string? UserName { get; set; }
	public string? Password { get; set; }
	public int? EmployeeId { get; set; } //HrRecorId

	public string? ProfilePicture { get; set; }
	public string? SIGNATURE { get; set; }
	public byte[]? SIGNATUREBuf_Ref_Check_by { get; set; }
	public int? Gender { get; set; }
	public string? EmployeeName { get; set; }
	public string? Employee_Id { get; set; } //EmployeeId in Employment
	public string? ShortName { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? LastUpdatedDate { get; set; }
	public DateTime? LastLoginDate { get; set; }
	public int? FailedLoginNo { get; set; }
	public bool IsActive { get; set; } = false;
	public bool? IsExpired { get; set; }
	public string? IsFirstLogin { get; set; }
	public string? CompanyName { get; set; }
	public string? FullLogoPath { get; set; }
	public bool? LogHourEnable { get; set; }
	public int? FiscalYearStart { get; set; }
	public int? AccessParentCompany { get; set; }

	public DateTime? LicenseExpiryDate { get; set; }
	public int? LicenseUserNo { get; set; }
	public int? TotalCount { get; set; }

	public int? ShiftId { get; set; }
	public int? BranchId { get; set; }
	public int? DepartmentId { get; set; }
	public string? Theme { get; set; }

	public string? AttendanceCardNo { get; set; }

	public List<GroupMemberDto>? GroupMembers { get; set; }

	public int? DefaultDashboard { get; set; }

	//new
	public string? DepartmentName { get; set; }
	public string? DESIGNATIONNAME { get; set; }
	public int? StateId { get; set; }
	public int? GroupId { get; set; }

	public string? IMEI { get; set; }
	public int? AssemblyInfoId { get; set; }

	public string? EmailAddress { get; set; }
}