using System;
using WPR.Entities.Abstractions.Base;

namespace WPR.Entities.Base;

/// <summary>
/// Реализация базовой сущности
/// </summary>
public abstract class Entity<TKey> : IEntity<TKey> where TKey : IComparable<TKey>
{
    
    public TKey Id { get; set; } = default!;

    public virtual bool Equals(IEntity<TKey>? other)
    {
        return other != null && Id.Equals(other.Id);
    }
}