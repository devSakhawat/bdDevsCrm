namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class GroupPermissionDto
{
  public int PermissionId { get; set; }
  public string PermissionTableName { get; set; }
  public int GroupId { get; set; }
  public int ReferenceID { get; set; }
  public int ParentPermission { get; set; }
}
