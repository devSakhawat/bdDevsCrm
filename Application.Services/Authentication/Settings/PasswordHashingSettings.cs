using System.ComponentModel.DataAnnotations;

namespace Application.Services.Authentication.Settings;

/// <summary>
/// Password hashing configuration settings
/// Binds to appsettings.json "PasswordHashing" section
/// </summary>
public class PasswordHashingSettings
{
	public const string SectionName = "PasswordHashing";

	/// <summary>
	/// Bcrypt work factor (cost parameter)
	/// Higher = more secure but slower
	/// Range: 10-31, Recommended: 12
	/// </summary>
	[Range(10, 31, ErrorMessage = "Bcrypt work factor must be between 10 and 31")]
	public int BcryptWorkFactor { get; set; } = 12;

	/// <summary>
	/// Enable automatic rehashing when work factor changes
	/// If true, passwords will be rehashed on next successful login
	/// </summary>
	public bool EnableAutoRehash { get; set; } = true;

	/// <summary>
	/// Log password hashing operations
	/// For security audit (does NOT log actual passwords)
	/// </summary>
	public bool EnableHashingLogs { get; set; } = true;
}