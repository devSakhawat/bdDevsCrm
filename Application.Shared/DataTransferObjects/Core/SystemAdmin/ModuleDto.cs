using Application.Shared.DataTransferObjects.Common;

namespace Application.Shared.DataTransferObjects.Core.SystemAdmin;

public class ModuleDto : CommonDto
{
  public int ModuleId { get; set; }

  public string ModuleName { get; set; } = null!;
}
