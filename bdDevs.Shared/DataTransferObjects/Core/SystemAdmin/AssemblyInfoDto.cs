namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;


public class AssemblyInfoDto
{
  public int AssemblyInfoId { get; set; }

  public string AssemblyTitle { get; set; } = null!;

  public string AssemblyDescription { get; set; } = null!;

  public string AssemblyCompany { get; set; } = null!;

  public string AssemblyProduct { get; set; } = null!;

  public string AssemblyCopyright { get; set; } = null!;

  public string AssemblyVersion { get; set; } = null!;

  public string ProductBanner { get; set; } = null!;

  /// <summary>
  /// false=Attedance by login inactive feature
  /// </summary>
  public bool IsAttendanceByLogin { get; set; }

  public string PoweredBy { get; set; } = null!;

  public string PoweredByUrl { get; set; } = null!;

  public bool IsDefault { get; set; }

  public string? ProductStyleSheet { get; set; }

  public string? ApiPath { get; set; }

  public string? CvBankPath { get; set; }
}
