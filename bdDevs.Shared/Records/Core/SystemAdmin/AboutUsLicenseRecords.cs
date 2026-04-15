namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new AboutUs License.
/// </summary>
public record CreateAboutUsLicenseRecord(
    int? AssemblyId,
    string? LicenseFor,
    string? ProductCode,
    string? CodeBaseVersion,
    string? LicenseNumber,
    string? LicenseType,
    string? Sbulicense,
    string? LocationLicense,
    string? UserLicense,
    string? ServerId,
    int? IsActive);

/// <summary>
/// Record for updating an existing AboutUs License.
/// </summary>
public record UpdateAboutUsLicenseRecord(
    int AboutUsLicenseId,
    int? AssemblyId,
    string? LicenseFor,
    string? ProductCode,
    string? CodeBaseVersion,
    string? LicenseNumber,
    string? LicenseType,
    string? Sbulicense,
    string? LocationLicense,
    string? UserLicense,
    string? ServerId,
    int? IsActive);

/// <summary>
/// Record for deleting an AboutUs License.
/// </summary>
public record DeleteAboutUsLicenseRecord(int AboutUsLicenseId);
