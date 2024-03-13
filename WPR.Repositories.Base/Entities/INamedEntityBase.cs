using System;

namespace WPR.Repositories.Base.Entities;

/// <summary>
/// Базовая именованная сущность
/// </summary>
public interface INamedEntityBase<TKey> : IEntityBase<TKey> where TKey : IComparable<TKey>
{
    /// <summary>
    /// Имя сущности.
    /// Обязательное свойство
    /// </summary>
    string Name { get; set; }
}