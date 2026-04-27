namespace bdDevsCrm.Shared.Settings;

public class AppSettings
{
  public const string SectionName = "AppSettings";

  /// <summary>
  /// Gets or sets the module ID reserved for the control panel section.
  /// </summary>
  public int ControlPanelModuleId { get; set; }

  public JwtSettings Jwt { get; set; } = new();
  public PasswordSettings Password { get; set; } = new();
  public CacheSettings Cache { get; set; } = new();
  public AuditSettings Audit { get; set; } = new();
  public TokenCleanupSettings TokenCleanup { get; set; } = new();
  public FileSettings File { get; set; } = new();
  public CorsSettings Cors { get; set; } = new();
}

public class JwtSettings
{
  public string SecretKey { get; set; } = string.Empty;
  public string Issuer { get; set; } = string.Empty;
  public string Audience { get; set; } = string.Empty;
  public int AccessTokenExpiryMinutes { get; set; } = 15;
  public int RefreshTokenExpiryDays { get; set; } = 7;
}

public class PasswordSettings
{
  public int SaltRounds { get; set; } = 12;
  public int MinLength { get; set; } = 8;
  public int MaxLength { get; set; } = 128;
  public int MaxWrongAttempts { get; set; } = 5;
  public int ExpiryDays { get; set; } = 90;
}

public class CacheSettings
{
  public bool EnableDistributedCache { get; set; } = false;
  public int UserCacheMinutes { get; set; } = 240;
  public int MenuCacheMinutes { get; set; } = 5;
  public int StaticDataHours { get; set; } = 24;
}

public class AuditSettings
{
  public bool EnableAuditMiddleware { get; set; } = true;
  public int QueueCapacity { get; set; } = 10_000;
  public int BatchSize { get; set; } = 100;
  public int FlushIntervalSeconds { get; set; } = 5;
  public int MaxBodyCaptureBytes { get; set; } = 4096;
}

public class TokenCleanupSettings
{
  public int IntervalHours { get; set; } = 24;
  public int RetryDelayMinutes { get; set; } = 5;
}

public class FileSettings
{
  public long MaxFileSizeBytes { get; set; } = 10_000_000;
  public string UploadPath { get; set; } = "wwwroot/Uploads";
  public string ApplicantPhotoRelativePath { get; set; } = "crm/applicants/photos";
  public string ApplicationDocumentRelativePath { get; set; } = "crm/applications/documents";
  public string[] AllowedImageExtensions { get; set; } = [".jpg", ".jpeg", ".png", ".webp"];
  public string[] AllowedDocumentExtensions { get; set; } = [".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png"];
}

public class CorsSettings
{
  public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
  public bool AllowCredentials { get; set; } = false;
  public int PreflightMaxAge { get; set; } = 0;
}


//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Application.Shared.Settings;

///// <summary>
///// Represents the application-level settings bound from the AppSettings section
///// of the configuration file.
///// </summary>
//public sealed class AppSettings
//{
//	public const string SectionName = "AppSettings";

//	/// <summary>
//	/// Gets or sets the module ID reserved for the control panel section.
//	/// </summary>
//	public int ControlPanelModuleId { get; set; }
//}
