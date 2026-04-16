namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class AppsTokenInfoDto
{
    public int AppsTokenInfoId { get; set; }
    public string? AppsUserId { get; set; }
    public string? EmployeeId { get; set; }
    public string? TokenNumber { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiredDate { get; set; }
}

public class AppsTokenInfoDDLDto
{
    public int AppsTokenInfoId { get; set; }
    public string? TokenNumber { get; set; }
    public string? AppsUserId { get; set; }
}
