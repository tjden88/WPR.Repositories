using System;

namespace WPR.Repositories.Base.Entities;

/// <summary>
/// Базовая сущность с пометкой удаления
/// </summary>
public interface IDeletedEntity<TKey> : IEntity<TKey> where TKey : IComparable<TKey>
{
    /// <summary>
    /// Удалена ли сущность
    /// </summary>
    bool IsDeleted { get; set; }
}