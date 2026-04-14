using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class EducationDetailsDto
{
  public List<EducationHistoryDto>? EducationHistory { get; set; }
  public int? TotalEducationRecords { get; set; }
}