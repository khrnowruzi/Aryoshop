using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Base;

/// <summary>
/// Interface for entities with a primary key.
/// </summary>
public interface IBaseEntity<TKey>
{
    TKey Id { get; set; }
}

/// <summary>
/// Interface for entities that support soft deletion.
/// </summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
}

/// <summary>
/// Combines entity base and soft deletion capability.
/// </summary>
public interface ISoftDeletableEntity<TKey> : ISoftDeletable, IBaseEntity<TKey> { }

/// <summary>
/// Interface for entities that track creation and update auditing.
/// </summary>
public interface IAuditable
{
    DateTime CreatedDate { get; set; }
    string CreatedBy { get; set; }
    DateTime? UpdatedDate { get; set; }
    string? UpdatedBy { get; set; }
}

/// <summary>
/// Combines auditing and soft deletion for an entity.
/// </summary>
public interface IAuditableEntity<TKey> : IAuditable, ISoftDeletableEntity<TKey> { }

/// <summary>
/// Base class for all entities with an identity key.
/// </summary>
public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
{
    [Key]
    public virtual TKey Id { get; set; } = default!;
}

/// <summary>
/// Base class for entities that support soft deletion.
/// </summary>
public abstract class SoftDeletableEntity<TKey> : BaseEntity<TKey>, ISoftDeletableEntity<TKey>
{
    public bool IsDeleted { get; set; } = false;
}

/// <summary>
/// Base class for auditable entities with soft deletion.
/// </summary>
public abstract class AuditableEntity<TKey> : SoftDeletableEntity<TKey>, IAuditableEntity<TKey>
{
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
}