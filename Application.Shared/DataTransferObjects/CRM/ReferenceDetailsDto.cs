using System.Collections.Generic;

namespace bdDevs.Shared.DataTransferObjects.CRM;

public class ReferenceDetailsDto
{
  public List<ApplicantReferenceDto>? References { get; set; }
  public int? TotalReferenceRecords { get; set; }
}