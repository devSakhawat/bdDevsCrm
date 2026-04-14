using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Shared.DataTransferObjects.Core.SystemAdmin;

public class ModuleForDDLDto
{
	public int ModuleId { get; set; }

	public string ModuleName { get; set; } = null!;
}
