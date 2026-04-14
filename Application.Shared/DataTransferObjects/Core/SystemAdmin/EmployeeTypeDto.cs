namespace Application.Shared.DataTransferObjects.Core.SystemAdmin;


public class EmployeeTypeDto
{
  public int EmployeeTypeId { get; set; }

  public string EmployeeTypeName { get; set; } = null!;

  public string? EmployeeTypeCode { get; set; }

  public int? IsActive { get; set; }

  public bool? IsContract { get; set; }

  public bool? IsNotAccess { get; set; }

  public int? IsPfApplicable { get; set; }

  public int? IsTrainee { get; set; }

  public int? IsRegular { get; set; }

  public int? IsUnion { get; set; }

  public int? IsProbationary { get; set; }

  public int? IsUnionProbationary { get; set; }

  public int? EmpTypeSortOrder { get; set; }

  public int? IsEwfApplicable { get; set; }
}
