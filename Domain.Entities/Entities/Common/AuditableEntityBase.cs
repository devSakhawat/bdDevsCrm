namespace Domain.Entities.Entities.Common;

/// <summary>
/// Optional base class. Use when your entities are able to inherit a base.
/// If inheritance not possible, implement IAuditableEntity on each entity.
/// </summary>
public abstract class AuditableEntityBase : IAuditableEntity
{
  public int? CreatedBy { get; set; }
  public DateTime? CreatedDate { get; set; }
  public int? UpdatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
}
