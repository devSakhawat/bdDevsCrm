using Domain.Entities.Entities.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace Infrastructure.Sql.Context.Interceptors;

/// <summary>
/// Intercepts SaveChanges to set audit fields on auditable entities.
/// </summary>
public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  /// <summary>
  /// Called during SaveChanges interception.
  /// </summary>
  public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
  {
    SetAuditFields(eventData.Context);
    return base.SavingChanges(eventData, result);
  }

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
      DbContextEventData eventData,
      InterceptionResult<int> result,
      CancellationToken cancellationToken = default)
  {
    SetAuditFields(eventData.Context);
    return base.SavingChangesAsync(eventData, result, cancellationToken);
  }

  private void SetAuditFields(DbContext? context)
  {
    if (context == null) return;

    var entries = context.ChangeTracker.Entries<IAuditableEntity>();

    var userId = TryCurrentUserId();

    foreach (var entry in entries)
    {
      if (entry.State == EntityState.Added)
      {
        entry.Entity.CreatedBy ??= userId;
        entry.Entity.CreatedDate ??= DateTime.UtcNow;

        entry.Entity.UpdatedBy = userId;
        entry.Entity.UpdatedDate = DateTime.UtcNow;
      }
      else if (entry.State == EntityState.Modified)
      {
        entry.Entity.UpdatedBy = userId;
        entry.Entity.UpdatedDate = DateTime.UtcNow;

        // We never overwrite CreatedBy/CreatedDate
        entry.Property(e => e.CreatedBy).IsModified = false;
        entry.Property(e => e.CreatedDate).IsModified = false;
      }
    }
  }

  /// <summary>
  /// Extract userId from current request context/users claims
  /// </summary>
  private int? TryCurrentUserId()
  {
    var accessor = _httpContextAccessor;
    var context = accessor?.HttpContext;
    var user = context?.User;
    if (user == null || !user.Identity.IsAuthenticated)
      return null;

    // Replace "UserId" with the correct claim type for your system if needed
    var userIdClaim = user.FindFirst("UserId")?.Value
                   ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (int.TryParse(userIdClaim, out var userId) && userId > 0)
      return userId;

    return null;
  }
}