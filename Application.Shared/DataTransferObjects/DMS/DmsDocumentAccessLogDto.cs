using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.DataTransferObjects.DMS;

public class DmsDocumentAccessLogDto
{
  public long LogId { get; set; }

  public int DocumentId { get; set; }

  public string? AccessedByUserId { get; set; }
  public string? IpAddress { get; set; }
  public string? DeviceInfo { get; set; }
  public string? MacAddress { get; set; }
  public string? Notes { get; set; }

  public DateTime? AccessDateTime { get; set; }

  public string Action { get; set; } = null!;
}
