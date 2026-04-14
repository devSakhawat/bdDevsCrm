using System.Collections.Generic;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class AdditionalDocumentsDto
{
  public List<AdditionalDocumentDto>? Documents { get; set; }
  public int? TotalDocumentRecords { get; set; }
}