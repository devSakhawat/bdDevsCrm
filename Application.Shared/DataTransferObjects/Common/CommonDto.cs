namespace Application.Shared.DataTransferObjects.Common;

public class CommonDto
{
  public int? CreatedBy { get; set; }
  public DateTimeOffset? CreatedDate { get; set; } = DateTime.Now;
  public int? UpdateBy { get; set; }
  public DateTimeOffset? UpdateDate { get; set; } = DateTime.Now;
  public int? StatusCode { get; set; }
  public string? ReponseMessage { get; set; }
}
