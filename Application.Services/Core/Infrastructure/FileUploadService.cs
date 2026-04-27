using bdDevsCrm.Shared.Settings;
using Domain.Contracts.Services.Core.Infrastructure;
using Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services.Core.Infrastructure;

/// <summary>
/// Shared service for validating and saving uploaded CRM files.
/// </summary>
public sealed class FileUploadService : IFileUploadService
{
  private readonly IWebHostEnvironment _environment;
  private readonly FileSettings _fileSettings;
  private readonly ILogger<FileUploadService> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="FileUploadService"/> class.
  /// </summary>
  public FileUploadService(
    IWebHostEnvironment environment,
    IOptions<AppSettings> appSettings,
    ILogger<FileUploadService> logger)
  {
    _environment = environment;
    _fileSettings = appSettings.Value.File;
    _logger = logger;
  }

  /// <inheritdoc />
  public Task<string> SaveApplicantPhotoAsync(IFormFile file, CancellationToken cancellationToken = default) =>
    ValidateAndSaveAsync(file, _fileSettings.AllowedImageExtensions, _fileSettings.ApplicantPhotoRelativePath, "photo", cancellationToken);

  /// <inheritdoc />
  public Task<string> SaveApplicationDocumentAsync(IFormFile file, CancellationToken cancellationToken = default) =>
    ValidateAndSaveAsync(file, _fileSettings.AllowedDocumentExtensions, _fileSettings.ApplicationDocumentRelativePath, "document", cancellationToken);

  /// <summary>
  /// Validates and saves the given file to the configured upload location.
  /// </summary>
  private async Task<string> ValidateAndSaveAsync(
    IFormFile? file,
    IEnumerable<string> allowedExtensions,
    string relativeDirectory,
    string fileType,
    CancellationToken cancellationToken)
  {
    if (file is null || file.Length <= 0)
      throw new BadRequestException($"A valid {fileType} file is required.");

    if (file.Length > _fileSettings.MaxFileSizeBytes)
      throw new BadRequestException($"{fileType} file size must be {_fileSettings.MaxFileSizeBytes / 1_000_000} MB or less.");

    string extension = Path.GetExtension(file.FileName);
    if (string.IsNullOrWhiteSpace(extension) || !allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
      throw new BadRequestException($"Unsupported {fileType} file type.");

    string physicalUploadRoot = ResolvePhysicalUploadRoot();
    string publicUploadRoot = ResolvePublicUploadRoot();
    string normalizedRelativeDirectory = NormalizePath(relativeDirectory);
    string targetDirectory = Path.Combine(physicalUploadRoot, normalizedRelativeDirectory);

    Directory.CreateDirectory(targetDirectory);

    string fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
    string fullPath = Path.Combine(targetDirectory, fileName);

    await using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
    await file.CopyToAsync(stream, cancellationToken);

    string publicPath = "/" + NormalizePath(Path.Combine(publicUploadRoot, normalizedRelativeDirectory, fileName));
    _logger.LogInformation("Saved uploaded {FileType} to {PublicPath}", fileType, publicPath);
    return publicPath;
  }

  /// <summary>
  /// Resolves the configured physical upload root path.
  /// </summary>
  private string ResolvePhysicalUploadRoot()
  {
    if (Path.IsPathRooted(_fileSettings.UploadPath))
      return _fileSettings.UploadPath;

    return Path.Combine(_environment.ContentRootPath, NormalizePath(_fileSettings.UploadPath));
  }

  /// <summary>
  /// Resolves the configured public upload root path.
  /// </summary>
  private string ResolvePublicUploadRoot()
  {
    string normalized = NormalizePath(_fileSettings.UploadPath);
    const string webRootPrefix = "wwwroot/";

    return normalized.StartsWith(webRootPrefix, StringComparison.OrdinalIgnoreCase)
      ? normalized[webRootPrefix.Length..]
      : normalized;
  }

  /// <summary>
  /// Normalizes a relative path to forward slashes without leading separators.
  /// </summary>
  private static string NormalizePath(string path) =>
    path.Replace("\\", "/").Trim('/');
}
