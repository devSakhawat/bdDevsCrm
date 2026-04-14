namespace Application.Shared.DataTransferObjects.Core.SystemAdmin;

public class GroupDto
{
  public int GroupId { get; set; }
  public int CompanyId { get; set; }
  public string GroupName { get; set; }
  public int IsDefault { get; set; }

  public List<GroupPermissionDto>? ModuleList { get; set; }

  public List<GroupPermissionDto>? MenuList { get; set; }

  public List<GroupPermissionDto>? AccessList { get; set; }

  public List<GroupPermissionDto>? StatusList { get; set; }

  public List<GroupPermissionDto>? ActionList { get; set; }

  public List<GroupPermissionDto>? ReportList { get; set; }

  public int? TotalCount { get; set; }
}
