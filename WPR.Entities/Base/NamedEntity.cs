using System;
using WPR.Entities.Abstractions.Base;

namespace WPR.Entities.Base;

/// <summary>
/// Реализация базовой именованной сущности
/// </summary>
public abstract class NamedEntity<TKey> : Entity<TKey>, INamedEntity<TKey> where TKey : IComparable<TKey>
{
    public string Name { get; set; } = null!;
}