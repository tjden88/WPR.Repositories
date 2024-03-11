using WPR.Entities.Abstractions.Db;

namespace WPR.Entities.Db;

/// <summary>
/// Базовая реализация удалённой сущности БД
/// </summary>
public abstract class DeletedDbEntity : DbEntity, IDeletedDbEntity
{
    public bool IsDeleted { get; set; }
}