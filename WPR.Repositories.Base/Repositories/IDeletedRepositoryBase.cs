using System;
using System.Threading;
using System.Threading.Tasks;
using WPR.Repositories.Base.Entities;

namespace WPR.Repositories.Base.Repositories;

/// <summary>
/// Репозиторий сущностей, помеченных как удалённые.
/// Методы получения возвращают коллекции и единичные элементы удалённых сущностей.
/// Методы добавления устанавливают свойство IsDeleted = true.
/// Методы обновления работают как обычно.
/// Методы удаления - удаляют сущность окончательно
/// </summary>
/// <typeparam name="TDeletedEntity">Тип, реализующий IDeletedEntityBase</typeparam>
/// <typeparam name="TKey">Тип идентификатора сущности</typeparam>
public interface IDeletedRepositoryBase<TDeletedEntity, in TKey> : IRepositoryBase<TDeletedEntity, TKey> where TDeletedEntity : IEntityBase<TKey>, IDeletedEntityBase<TKey> where TKey : IComparable<TKey>
{

    /// <summary>
    /// Восстановить удалённую сущность
    /// </summary>
    /// <returns>null, если не удалось</returns>
    Task<TDeletedEntity?> RestoreAsync(TKey id, CancellationToken Cancel = default);
}