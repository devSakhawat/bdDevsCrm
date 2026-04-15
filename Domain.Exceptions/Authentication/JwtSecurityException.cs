using Microsoft.IdentityModel.Tokens;

namespace Domain.Exceptions;

public sealed class JwtSecurityException : SecurityTokenValidationException
{
  public JwtSecurityException()
  {
  }

  public JwtSecurityException(string message) : base(message)
  {
  }

  public JwtSecurityException(string message, Exception innerException) : base(message, innerException)
  {
  }
}
