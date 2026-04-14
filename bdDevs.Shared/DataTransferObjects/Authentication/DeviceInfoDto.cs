using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdDevs.Shared.DataTransferObjects.Authentication;

public class LoginDeviceHistoryDto
{
  public int LoginDeviceHistoryId { get; set; }
  public string IpAddress { get; set; }
  public string UserAgent { get; set; }
  public string DeviceType { get; set; }
  public string Browser { get; set; }
  public string OperatingSystem { get; set; }
  public DateTime Timestamp { get; set; }
}