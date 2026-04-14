using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.DataTransferObjects.DMS;

public class DmsDocumentDDL
{
  public int DocumentId { get; set; }
  public string Title { get; set; } = null!;
}