using System.Collections.Generic;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class ReferenceDetailsDto
{
  public List<ApplicantReferenceDto>? References { get; set; }
  public int? TotalReferenceRecords { get; set; }
}