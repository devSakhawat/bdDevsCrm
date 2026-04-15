namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating new assembly info.
/// </summary>
public record CreateAssemblyInfoRecord(
    string AssemblyTitle,
    string AssemblyDescription,
    string AssemblyCompany,
    string AssemblyProduct,
    string AssemblyCopyright,
    string AssemblyVersion,
    string ProductBanner,
    bool IsAttendanceByLogin,
    string PoweredBy,
    string PoweredByUrl,
    bool IsDefault,
    string? ProductStyleSheet,
    string? ApiPath,
    string? CvBankPath);

/// <summary>
/// Record for updating existing assembly info.
/// </summary>
public record UpdateAssemblyInfoRecord(
    int AssemblyInfoId,
    string AssemblyTitle,
    string AssemblyDescription,
    string AssemblyCompany,
    string AssemblyProduct,
    string AssemblyCopyright,
    string AssemblyVersion,
    string ProductBanner,
    bool IsAttendanceByLogin,
    string PoweredBy,
    string PoweredByUrl,
    bool IsDefault,
    string? ProductStyleSheet,
    string? ApiPath,
    string? CvBankPath);

/// <summary>
/// Record for deleting assembly info.
/// </summary>
public record DeleteAssemblyInfoRecord(int AssemblyInfoId);
