using System;
using WPR.Repositories.Base.Entities;

namespace WPR.Repositories.Base.Models;

/// <summary>
/// Реализация базовой сущности
/// </summary>
public abstract class EntityBase<TKey> : IEntityBase<TKey> where TKey : IComparable<TKey>
{
    
    public TKey Id { get; set; } = default!;

    public virtual bool Equals(IEntityBase<TKey>? other)
    {
        return other != null && Id.Equals(other.Id);
    }
}