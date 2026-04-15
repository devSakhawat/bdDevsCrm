using Domain.Exceptions.Forbidden;

namespace Domain.Exceptions.Authentication;

public sealed class AccessDeniedException : ForbiddenException
{
  public AccessDeniedException() : base("Access is denied due to insufficient permissions.")
  {

  }
}