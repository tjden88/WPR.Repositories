using System;
using WPR.Entities.Abstractions.Base;

namespace WPR.Entities.Base;

/// <summary>
/// Реализация базовой удаляемой сущности
/// </summary>
public abstract class DeletedEntity<TKey> : Entity<TKey>, IDeletedEntity<TKey> where TKey : IComparable<TKey>
{
    public bool IsDeleted { get; set; }
}