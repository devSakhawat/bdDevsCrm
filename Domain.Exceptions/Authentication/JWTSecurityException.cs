using Microsoft.IdentityModel.Tokens;

namespace Domain.Exceptions;

public sealed class JWTSecurityException : SecurityTokenValidationException
{
  public JWTSecurityException()
  {
  }

  public JWTSecurityException(string message) : base(message)
  {
  }

  public JWTSecurityException(string message, Exception innerException) : base(message, innerException)
  {
  }
}
