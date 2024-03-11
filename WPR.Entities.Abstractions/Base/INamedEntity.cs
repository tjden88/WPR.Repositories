using System;

namespace WPR.Entities.Abstractions.Base;

/// <summary>
/// Базовая именованная сущность
/// </summary>
public interface INamedEntity<TKey> : IEntity<TKey> where TKey : notnull, IComparable<TKey>
{
    /// <summary>
    /// Имя сущности.
    /// Обязательное свойство
    /// </summary>
    string Name { get; set; }
}