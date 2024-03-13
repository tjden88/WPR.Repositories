using System;
using WPR.Repositories.Base.Entities;

namespace WPR.Repositories.Base.Models;

/// <summary>
/// Реализация базовой удаляемой сущности
/// </summary>
public abstract class DeletedEntity<TKey> : Entity<TKey>, IDeletedEntity<TKey> where TKey : IComparable<TKey>
{
    public bool IsDeleted { get; set; }
}