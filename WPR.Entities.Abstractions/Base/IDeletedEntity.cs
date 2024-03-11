using System;

namespace WPR.Entities.Abstractions.Base;

/// <summary>
/// Базовая сущность с пометкой удаления
/// </summary>
public interface IDeletedEntity<TKey> : IEntity<TKey> where TKey : notnull, IComparable<TKey>
{
    /// <summary>
    /// Удалена ли сущность
    /// </summary>
    bool IsDeleted { get; set; }
}