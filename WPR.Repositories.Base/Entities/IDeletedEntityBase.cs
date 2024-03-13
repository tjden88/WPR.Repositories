using System;

namespace WPR.Repositories.Base.Entities;

/// <summary>
/// Базовая сущность с пометкой удаления
/// </summary>
public interface IDeletedEntityBase<TKey> : IEntityBase<TKey> where TKey : IComparable<TKey>
{
    /// <summary>
    /// Удалена ли сущность
    /// </summary>
    bool IsDeleted { get; set; }
}