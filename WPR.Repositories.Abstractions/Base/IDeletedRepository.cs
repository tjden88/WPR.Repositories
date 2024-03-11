using System;
using System.Threading;
using System.Threading.Tasks;
using WPR.Entities.Abstractions.Base;

namespace WPR.Repositories.Abstractions.Base;

/// <summary>
/// Репозиторий сущностей, помеченных как удалённые.
/// Методы получения возвращают коллекции и единичные элементы удалённых сущностей.
/// Методы добавления устанавливают свойство IsDeleted = true.
/// Методы обновления работают как обычно.
/// Методы удаления - удаляют сущность окончательно
/// </summary>
/// <typeparam name="TDeletedEntity">Тип, реализующий IDeletedEntity</typeparam>
/// <typeparam name="TKey">Тип идентификатора сущности</typeparam>
public interface IDeletedRepository<TDeletedEntity, in TKey> : IRepository<TDeletedEntity, TKey> where TDeletedEntity : IEntity<TKey>, IDeletedEntity<TKey> where TKey : IComparable<TKey>
{

    /// <summary>
    /// Восстановить удалённую сущность
    /// </summary>
    /// <returns>null, если не удалось</returns>
    Task<TDeletedEntity?> RestoreAsync(TKey id, CancellationToken Cancel = default);
}