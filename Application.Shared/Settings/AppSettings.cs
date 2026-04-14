using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Shared.Settings;

/// <summary>
/// Represents the application-level settings bound from the AppSettings section
/// of the configuration file.
/// </summary>
public sealed class AppSettings
{
	public const string SectionName = "AppSettings";

	/// <summary>
	/// Gets or sets the module ID reserved for the control panel section.
	/// </summary>
	public int ControlPanelModuleId { get; set; }
}
