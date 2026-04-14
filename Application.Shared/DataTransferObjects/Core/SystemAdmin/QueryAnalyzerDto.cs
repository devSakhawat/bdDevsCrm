namespace Application.Shared.DataTransferObjects.Core.SystemAdmin;

public class QueryAnalyzerDto
{
  public int ReportHeaderId { get; set; }
  public string ReportHeader { get; set; }
  public string ReportTitle { get; set; }

  public int QueryType { get; set; }
  public string QueryTypeName { get; set; }
  public string QueryText { get; set; }

  public int IsActive { get; set; }

  public string OrderByColumn { get; set; }
}
