using Domain.Exceptions.Forbidden_403;

namespace Domain.Exceptions.Authentication;

public sealed class AccessDeniedException : ForbiddenException
{
  public AccessDeniedException() : base("Access is denied due to insufficient permissions.")
  {

  }
}