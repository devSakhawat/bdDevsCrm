namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class AccessRestrictionDto
{
  public int HrRecordId { get; set; }
  public string EmployeeId { get; set; }
  public int ReferenceId { get; set; }
  public int ParentReference { get; set; }
  public int ChiledParentReference { get; set; }
  public int ReferenceType { get; set; }
  public DateTime AccessDate { get; set; }
  public int AccessBy { get; set; }
  public int RestrictionType { get; set; }
  public int GroupId { get; set; }
}
