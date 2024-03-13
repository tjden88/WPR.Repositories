using System;

namespace WPR.Entities.Abstractions.Base;

/// <summary>
/// Базовая сущность
/// </summary>
/// <typeparam name="TKey">Тип идентификатора</typeparam>
public interface IEntity<TKey> : IEquatable<IEntity<TKey>> where TKey : IComparable<TKey>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    TKey Id { get; set; }
}