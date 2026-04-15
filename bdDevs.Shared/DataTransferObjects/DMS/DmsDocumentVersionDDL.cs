using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevs.Shared.DataTransferObjects.DMS;

public class DmsDocumentVersionDDL
{
  public int VersionId { get; set; }
  public int DocumentId { get; set; }
  public int VersionNumber { get; set; }
}
