using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class CRMInstituteTypeDto
{
  public int InstituteTypeId { get; set; }

  public string InstituteTypeName { get; set; } = null!;
}
