using System.ComponentModel.DataAnnotations;
using WPR.Entities.Abstractions.Db;

namespace WPR.Entities.Db;

/// <summary>
/// Базовая реализация именованной сущности БД
/// </summary>
public abstract class NamedDbEntity : DbEntity, INamedDbEntity
{
    [Required]
    public string Name { get; set; } = null!;
}