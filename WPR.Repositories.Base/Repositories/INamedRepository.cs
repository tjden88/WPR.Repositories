using System;
using System.Threading;
using System.Threading.Tasks;
using WPR.Repositories.Base.Entities;

namespace WPR.Repositories.Base.Repositories;

public interface INamedRepository<TEntity, in TKey> : IRepository<TEntity, TKey> where TEntity : INamedEntity<TKey> where TKey : IComparable<TKey>
{
    /// <summary>
    /// Существует ли сущность в репозитории
    /// </summary>
    /// <param name="Name">Имя сущности</param>
    /// <param name="Cancel">Токен отмены</param>
    /// <returns>Истина, если сущность есть в репозитории</returns>
    Task<bool> ExistNameAsync(string Name, CancellationToken Cancel = default);


    /// <summary>
    /// Получить сущность по имени
    /// </summary>
    /// <param name="Name">Имя сущности</param>
    /// <param name="Cancel">Токен отмены</param>
    /// <returns>null, если сущность не найдена</returns>
    Task<TEntity?> GetByNameAsync(string Name, CancellationToken Cancel = default);


    /// <summary>
    /// Переименовать сущность
    /// </summary>
    /// <param name="newName">Новое имя сущности</param>
    /// <param name="Cancel">Токен отмены</param>
    /// <param name="id">Идентификатор</param>
    Task<bool> UpdateNameAsync(TKey id, string newName, CancellationToken Cancel = default);
}