using bdDevs.Shared.DataTransferObjects.Common;

namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class ModuleDto : CommonDto
{
  public int ModuleId { get; set; }

  public string ModuleName { get; set; } = null!;
}
