using Application.Services.Authentication.Settings;
using BCrypt.Net;
using Domain.Contracts.Infrastructure.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services.Authentication.Security;
/// <summary>
/// Bcrypt password hashing implementation
/// Thread-safe, uses BCrypt.Net-Next library
/// </summary>
public class BcryptPasswordHasher : IPasswordHasher
{
  private readonly ILogger<BcryptPasswordHasher> _logger;
  private readonly PasswordHashingSettings _settings;

  public BcryptPasswordHasher(ILogger<BcryptPasswordHasher> logger, IOptions<PasswordHashingSettings> settings)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

    // Validate settings on construction
    if (_settings.BcryptWorkFactor < 10 || _settings.BcryptWorkFactor > 31)
    {
      throw new InvalidOperationException(
        $"Bcrypt work factor must be between 10 and 31. Current value: {_settings.BcryptWorkFactor}");
    }
  }

  /// <summary>
  /// Hash password using bcrypt
  /// </summary>
  public string HashPassword(string password)
  {
    // ============================================================
    // Input Validation
    // ============================================================
    if (string.IsNullOrWhiteSpace(password))
    {
      _logger.LogWarning("Attempted to hash null or empty password");
      throw new ArgumentException("Password cannot be null or empty", nameof(password));
    }

    if (password.Length > 72)
    {
      _logger.LogWarning("Password exceeds bcrypt maximum length of 72 characters");
      throw new ArgumentException(
        "Password cannot exceed 72 characters (bcrypt limitation)",
        nameof(password));
    }

    try
    {
      // ============================================================
      // Hash Generation
      // ============================================================
      _logger.LogDebug("Hashing password with work factor {WorkFactor}", _settings.BcryptWorkFactor);

      var hash = BCrypt.Net.BCrypt.HashPassword(
        password,
        workFactor: _settings.BcryptWorkFactor
      );

      _logger.LogInformation("Password hashed successfully");

      return hash;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Unexpected error while hashing password");
      throw new InvalidOperationException("Failed to hash password", ex);
    }
  }

  /// <summary>
  /// Verify password against bcrypt hash
  /// Uses constant-time comparison (timing attack resistant)
  /// </summary>
  public bool VerifyPassword(string password, string hash)
  {
    // ============================================================
    // Input Validation
    // ============================================================
    if (string.IsNullOrWhiteSpace(password))
    {
      _logger.LogWarning("Attempted to verify null or empty password");
      return false; // Don't throw, just return false
    }

    if (string.IsNullOrWhiteSpace(hash))
    {
      _logger.LogWarning("Attempted to verify against null or empty hash");
      return false;
    }

    try
    {
      // ============================================================
      // Verification (Constant-Time)
      // ============================================================
      _logger.LogDebug("Verifying password");

      bool isValid = BCrypt.Net.BCrypt.Verify(password, hash);

      if (isValid)
      {
        _logger.LogInformation("Password verification successful");
      }
      else
      {
        _logger.LogWarning("Password verification failed - incorrect password");
      }

      return isValid;
    }
    catch (BCrypt.Net.SaltParseException ex)
    {
      // Invalid hash format (corrupted or not bcrypt hash)
      _logger.LogError(ex, "Invalid hash format - not a valid bcrypt hash");
      return false;
    }
    catch (Exception ex)
    {
      // Unexpected error - fail securely
      _logger.LogError(ex, "Unexpected error during password verification");
      return false; // Fail securely (deny access on error)
    }
  }

  /// <summary>
  /// Check if hash needs rehashing
  /// Useful when work factor is increased in configuration
  /// </summary>
  public bool NeedsRehash(string hash)
  {
    if (string.IsNullOrWhiteSpace(hash))
    {
      return true;
    }

    try
    {
      // Extract work factor from hash
      // Bcrypt hash format: $2a$12$...
      // Position 4-5 contains work factor
      var parts = hash.Split('$');
      if (parts.Length < 4)
      {
        _logger.LogWarning("Invalid bcrypt hash format");
        return true;
      }

      if (int.TryParse(parts[2], out int currentWorkFactor))
      {
        bool needsRehash = currentWorkFactor < _settings.BcryptWorkFactor;

        if (needsRehash)
        {
          _logger.LogInformation(
            "Hash needs rehashing: current={Current}, target={Target}",
            currentWorkFactor,
            _settings.BcryptWorkFactor);
        }

        return needsRehash;
      }

      return true; // Can't determine, safer to rehash
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error checking if hash needs rehashing");
      return true; // Safer to rehash
    }
  }

}