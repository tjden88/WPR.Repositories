﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WPR.Entities.Abstractions.Base;
using WPR.Repositories.Abstractions.Paging;

namespace WPR.Repositories.Abstractions.Base;

/// <summary>
/// Базовый интерфейс репозитория сущностей
/// </summary>
/// <typeparam name="TEntity">Сущность</typeparam>
/// <typeparam name="TKey">Тип идентификатора сущности</typeparam>
public interface IRepository<TEntity, in TKey> where TEntity : IEntity<TKey> where TKey : IComparable<TKey>
{

    /// <summary>
    /// Получить все элементы репозитория
    /// </summary>
    /// <param name="OrderExpression">Выражение сортировки изначальной выборки. При отсутствии выполняется сортировка по Id</param>
    /// <param name="Cancel">Токен отмены</param>
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, object>>? OrderExpression = null, CancellationToken Cancel = default);


    /// <summary>
    /// Асинхронная выборка элементов с фильтром
    /// </summary>
    /// <param name="Filter">Выражение фильтра</param>
    /// <param name="Cancel">Токен отмены</param>
    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> Filter, CancellationToken Cancel = default);


    /// <summary>
    /// Постраничная выборка элементов с сортировкой данных
    /// </summary>
    /// <param name="PageIndex">Номер страницы (0 или больше)</param>
    /// <param name="PageSize">Размер страницы</param>
    /// <param name="OrderExpression">Выражение сортировки изначальной выборки. При отсутствии выполняется сортировка по Id</param>
    /// <param name="Ascending">Порядок сортировки (по умолчанию - по возрастанию)</param>
    /// <param name="Cancel">Токен отмены</param>
    Task<IPage<TEntity>> GetPageAsync(int PageIndex, int PageSize, Expression<Func<TEntity, object>>? OrderExpression = null, bool Ascending = true, CancellationToken Cancel = default);


    /// <summary>
    /// Постраничная выборка элементов с расширенной сортировкой и фильтрацией данных
    /// </summary>
    /// <param name="Filter">Фильтрация и сортировка выборки</param>
    /// <param name="Cancel">Токен отмены</param>
    Task<IPage<TEntity>> GetPageAsync(IPageFilter<TEntity> Filter, CancellationToken Cancel = default);


    /// <summary> Получить количество сущностей </summary>
    Task<int> CountAsync(CancellationToken Cancel = default);


    /// <summary> Получить количество сущностей с учётом фильтра </summary>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> Filter, CancellationToken Cancel = default);


    /// <summary>
    /// Существует ли сущность в репозитории
    /// </summary>
    /// <param name="id">идентификатор сущности</param>
    /// <param name="Cancel">Токен отмены</param>
    /// <returns>Истина, если сущность есть в репозитории</returns>
    Task<bool> ExistAsync(TKey id, CancellationToken Cancel = default);


    /// <summary>
    /// Получить сущность по идентификатору
    /// </summary>
    /// <param name="id">Id сущности</param>
    /// <param name="Cancel">Токен отмены</param>
    /// <returns>null, если сущность не найдена</returns>
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken Cancel = default);


    /// <summary>
    /// Получить единственную сущность по предикату
    /// </summary>
    /// <param name="Match">Предикат выборки</param>
    /// <param name="Cancel">Токен отмены</param>
    /// <returns>null, если ничего не нашлось, либо первая подходящая</returns>
    Task<TEntity?> GetOne(Expression<Func<TEntity, bool>> Match, CancellationToken Cancel = default);


    /// <summary>
    /// Добавить сущность в репозиторий
    /// </summary>
    /// <param name="item">Добавляемая сущность</param>
    /// <param name="Cancel">Токен отмены</param>
    Task<TEntity?> AddAsync(TEntity item, CancellationToken Cancel = default);


    /// <summary>
    /// Добавить несколько сущностей
    /// </summary>
    /// <param name="items">Коллекция добавляемых сущностей</param>
    /// <param name="Cancel">Токен отмены</param>
    Task<int> AddRangeAsync(IEnumerable<TEntity> items, CancellationToken Cancel = default);


    /// <summary>
    /// Обновить сущность в репозитории
    /// </summary>
    /// <param name="item">Изменяемая сущность</param>
    /// <param name="Cancel">Токен отмены</param>
    Task<bool> UpdateAsync(TEntity item, CancellationToken Cancel = default);


    /// <summary>
    /// Обновить несколько сущностей
    /// </summary>
    /// <param name="items">Коллекция обновляемых сущностей</param>
    /// <param name="Cancel">Токен отмены</param>
    Task<int> UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken Cancel = default);


    /// <summary>
    /// Обновить только одно свойство сущности репозитория
    /// </summary>
    /// <param name="item">Экземпляр сущности</param>
    /// <param name="property">Выражение - свойство для обновления</param>
    /// <param name="Cancel">Токен отмены</param>
    Task<bool> UpdatePropertyAsync(TEntity item, Expression<Func<TEntity, object>> property, CancellationToken Cancel = default);


    /// <summary>
    /// Обновить только выбранные свойства сущности репозитория
    /// </summary>
    /// <param name="item">Экземпляр сущности</param>
    /// <param name="properties">Выражения - свойства для обновления</param>
    /// <param name="Cancel">Токен отмены</param>
    Task<bool> UpdatePropertiesAsync(TEntity item, IEnumerable<Expression<Func<TEntity, object>>> properties, CancellationToken Cancel = default);


    /// <summary>
    /// Удалить сущность из репозитория
    /// </summary>
    /// <param name="id">Id удаляемой сущности</param>
    /// <param name="Cancel">Токен отмены</param>
    Task<bool> DeleteAsync(TKey id, CancellationToken Cancel = default);


    /// <summary>
    /// Удалить несколько сущностей
    /// </summary>
    /// <param name="ids">Коллекция идентификаторов удаляемых сущностей</param>
    /// <param name="Cancel">Токен отмены</param>
    Task<int> DeleteRangeAsync(IEnumerable<TKey> ids, CancellationToken Cancel = default);
}