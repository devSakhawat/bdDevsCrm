using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevs.Shared.DataTransferObjects.DMS;

public class DmsDocumentTagDto
{
  public int TagId { get; set; }

  public string Name { get; set; } = null!;

  //public virtual ICollection<DmsDocument> Document { get; set; } = new List<DmsDocument>();
}
