using System;
using WPR.Repositories.Base.Entities;

namespace WPR.Repositories.Base.Models;

/// <summary>
/// Реализация базовой именованной сущности
/// </summary>
public abstract class NamedEntityBase<TKey> : EntityBase<TKey>, INamedEntityBase<TKey> where TKey : IComparable<TKey>
{
    public string Name { get; set; } = null!;
}