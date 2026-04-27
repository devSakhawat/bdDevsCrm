using Microsoft.AspNetCore.Http;

namespace Domain.Contracts.Services.Core.Infrastructure;

/// <summary>
/// Shared service for validating and saving uploaded CRM files.
/// </summary>
public interface IFileUploadService
{
  /// <summary>
  /// Validates and saves an applicant photo file.
  /// </summary>
  /// <param name="file">The uploaded photo file.</param>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
  /// <returns>The public relative file path.</returns>
  Task<string> SaveApplicantPhotoAsync(IFormFile file, CancellationToken cancellationToken = default);

  /// <summary>
  /// Validates and saves an application document file.
  /// </summary>
  /// <param name="file">The uploaded document file.</param>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
  /// <returns>The public relative file path.</returns>
  Task<string> SaveApplicationDocumentAsync(IFormFile file, CancellationToken cancellationToken = default);
}
