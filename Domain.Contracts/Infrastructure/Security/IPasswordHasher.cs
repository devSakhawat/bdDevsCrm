using System;

namespace Domain.Contracts.Infrastructure.Security;

/// <summary>
/// Password hashing service interface
/// Supports bcrypt hashing with configurable work factor
/// </summary>
public interface IPasswordHasher
{
	/// <summary>
	/// Hash a plain text password using bcrypt
	/// </summary>
	/// <param name="password">Plain text password</param>
	/// <returns>Bcrypt hash string (contains salt + hash)</returns>
	/// <exception cref="ArgumentException">If password is null/empty</exception>
	string HashPassword(string password);

	/// <summary>
	/// Verify a password against a stored hash
	/// </summary>
	/// <param name="password">Plain text password to verify</param>
	/// <param name="hash">Stored bcrypt hash</param>
	/// <returns>True if password matches, false otherwise</returns>
	bool VerifyPassword(string password, string hash);

	/// <summary>
	/// Check if a hash needs rehashing (work factor changed)
	/// </summary>
	/// <param name="hash">Existing bcrypt hash</param>
	/// <returns>True if rehashing recommended</returns>
	bool NeedsRehash(string hash);
}