namespace bdDevs.Shared.DataTransferObjects.Core.HR;

public class DepartmentDto
{
  public int DepartmentId { get; set; }

  public string? DepartmentName { get; set; }

  public string? DepartmentCode { get; set; }

  public int? IsCostCentre { get; set; }

  public int? IsActive { get; set; }

  public int? CreateBy { get; set; }

  public DateTime? CreateDate { get; set; }

  public int? UpdateBy { get; set; }

  public DateTime? UpdateDate { get; set; }
}