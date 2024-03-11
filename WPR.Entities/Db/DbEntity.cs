using System.ComponentModel.DataAnnotations;
using WPR.Entities.Abstractions.Base;
using WPR.Entities.Abstractions.Db;
namespace WPR.Entities.Db;

/// <summary>
/// Базовая реализация сущности БД
/// </summary>
public abstract class DbEntity : IDbEntity
{
    [Key]
    public int Id { get; set; }

    public virtual bool Equals(IEntity<int>? other) => other != null && other.Id.Equals(Id);
}