namespace bdDevs.Shared.DataTransferObjects.Authentication;

/// <summary>
/// Result of login validation with detailed status
/// </summary>
public class LoginValidationResult
{
	public bool IsSuccess { get; set; }
	public LoginValidationStatus Status { get; set; }
	public string Message { get; set; }
	public UserSessionData? UserSession { get; set; }
	//public AttendanceInfo? Attendance { get; set; }
}

/// <summary>
/// Login validation status codes
/// </summary>
public enum LoginValidationStatus
{
	Success = 0,
	Failed = 1,
	Inactive = 2,
	Expired = 3,
	PasswordChangeRequired = 4,
	AttendanceShort = 5,
	AttendanceLeave = 6,
	AccountLocked = 7
}

/// <summary>
/// User session data after successful login
/// </summary>
public class UserSessionData
{
	public int UserId { get; set; }
	public string LoginId { get; set; }
	public string UserName { get; set; }
	public int CompanyId { get; set; }
	public int EmployeeId { get; set; }
	public string CompanyName { get; set; }
	public string FullLogoPath { get; set; }
	public bool LogHourEnable { get; set; }
	public DateTime? ExpiryDate { get; set; }
	public int LicenseUserCount { get; set; }
	public int FiscalYearStart { get; set; }
	public int BranchId { get; set; }
	public int AccessParentCompany { get; set; }
	public string ProfilePicture { get; set; }
	public int Gender { get; set; }
	public int DefaultDashboard { get; set; }
	public string Employee_Id { get; set; }
	public int DepartmentId { get; set; }
}

/// <summary>
/// Attendance information from login
/// </summary>
public class AttendanceInfo
{
	public string Message { get; set; }
	public DateTime AttendanceDate { get; set; }
	public string Status { get; set; } // "IN", "OUT", "SHORT", "LEAVE"
									   // Add other attendance properties as needed
}