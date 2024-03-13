using System;

namespace WPR.Repositories.Base.Entities;

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