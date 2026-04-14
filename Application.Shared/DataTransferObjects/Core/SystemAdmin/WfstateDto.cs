namespace Application.Shared.DataTransferObjects.Core.SystemAdmin;


public class WfStateDto
{
  public int RowIndex { get; set; }
  public int WfStateId { get; set; }

  public string? StateName { get; set; } = null!;

  public int? MenuId { get; set; }

  public bool? IsDefaultStart { get; set; }

  public int? IsClosed { get; set; }

  public int? Sequence { get; set; }

  public int? TotalCount { get; set; }
  public string? MenuName { get; set; }

  public int? ModuleId { get; set; }
  public string? ModuleName{ get; set; }

  public int? ClosingStateId { get; set; }
  public string? ClosingStateName { get; set; }
}