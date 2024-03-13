using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WPR.Repositories.Base.Entities;
using WPR.Repositories.Base.Models;
using WPR.Repositories.Base.Paging;
using WPR.Repositories.Base.Repositories;
using WPR.Repositories.EntityFramework.Base.Resolver;

namespace WPR.Repositories.EntityFramework.Base.Repositories;

/// <summary>
/// Репозиторий сущностей БД
/// </summary>
/// <typeparam name="T">Сущность БД</typeparam>
/// <typeparam name="TKey">Тип первичного ключа</typeparam>
public class DbRepositoryBase<T, TKey> : IRepositoryBase<T, TKey> where T : EntityBase<TKey>, new() where TKey : IComparable<TKey>
{
    private readonly DbContext _Db;

    /// <summary>
    /// Репозиторий сущностей БД
    /// </summary>
    /// <typeparam name="T">Сущность БД</typeparam>
    public DbRepositoryBase(IDbResolver DbResolver)
    {
        _Db = DbResolver.GetDbContext<T>();
    }

    private static bool IsDeletedEntity => typeof(IDeletedEntityBase<TKey>).IsAssignableFrom(typeof(T));


    /// <summary> Набор данных БД </summary>
    protected DbSet<T> Set => _Db.Set<T>();


    #region IRepository
    protected virtual IQueryable<T> Items
    {
        get
        {
            IQueryable<T> itemsQuery = Set;

            if (IsDeletedEntity)
                itemsQuery = itemsQuery.Where(item => !((IDeletedEntityBase<int>)item).IsDeleted);

            return itemsQuery;
        }
    }


    public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>? OrderExpression = null, CancellationToken Cancel = default) => 
        await Items.OrderBy(OrderExpression ?? (item => item.Id)).ToArrayAsync(Cancel);

    public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> Filter, CancellationToken Cancel = default) =>
        await Items.Where(Filter).ToArrayAsync(Cancel);


    public virtual async Task<IPage<T>> GetPageAsync(int PageIndex, int PageSize, Expression<Func<T, object>>? OrderExpression = null, bool Ascending = true, CancellationToken Cancel = default) =>
        await GetPageAsync(new PageFilter<T>
        {
            PageIndex = PageIndex,
            PageSize = PageSize,
            OrderBy = OrderExpression is null
                ? null
                : new PageOrderInfo<T>(OrderExpression, Ascending),
        }, Cancel)
            .ConfigureAwait(false);


    public virtual async Task<IPage<T>> GetPageAsync(IPageFilter<T> Filter, CancellationToken Cancel = default)
    {
        var pageIndex = Filter.PageIndex;
        var pageSize = Filter.PageSize;

        if (pageIndex < 1)
            throw new ArgumentOutOfRangeException(nameof(pageIndex), "Номер страницы не может быть меньше 1");

        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля");

        var items = Items.AsQueryable();

        var query = Filter.Filter is null
            ? items
            : items.Where(Filter.Filter);

        var count = query.Count();

        if (Filter.OrderBy is { } orderBy)
            query = orderBy.IsAscending
                ? query.OrderBy(orderBy.OrderExpression)
                : query.OrderByDescending(orderBy.OrderExpression);
        else
            query = query.OrderBy(item => item.Id);


        if (Filter.ThenOrderBy?.Any() == true)
            foreach (var thenOrder in Filter.ThenOrderBy)
                query = thenOrder.IsAscending
                    ? ((IOrderedQueryable<T>)query).ThenBy(thenOrder.OrderExpression)
                    : ((IOrderedQueryable<T>)query).ThenByDescending(thenOrder.OrderExpression);


        if (pageIndex > 1)
            query = query.Skip((pageIndex - 1) * pageSize);

        query = query.Take(pageSize);

        return new Page<T>(await query.ToArrayAsync(Cancel).ConfigureAwait(false), count, pageIndex, pageSize);
    }


    public virtual async Task<int> CountAsync(CancellationToken Cancel = default) =>await Items.CountAsync(Cancel);

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> Filter, CancellationToken Cancel = default) => await Items.CountAsync(Filter, cancellationToken: Cancel);


    public virtual async Task<bool> ExistAsync(TKey id, CancellationToken Cancel = default) => await Items.AnyAsync(item => Equals(id, item.Id), cancellationToken: Cancel);


    public virtual async Task<T?> GetByIdAsync(TKey id, CancellationToken Cancel = default) =>await Items.FirstOrDefaultAsync(item => item.Id.Equals(id), cancellationToken: Cancel);


    public virtual async Task<T?> AddAsync(T item, CancellationToken Cancel = default)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        _Db.Entry(item).State = EntityState.Added;

        try
        {
            var result = await _Db.SaveChangesAsync(Cancel).ConfigureAwait(false) > 0;

            return result
                ? item
                : default;
        }
        catch (Exception)
        {
            return default;
        }
    }


    public virtual async Task<int> AddRangeAsync(IEnumerable<T> items, CancellationToken Cancel = default)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        foreach (var item in items)
            _Db.Entry(item).State = EntityState.Added;

        try
        {
            return await _Db.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }
        catch (Exception)
        {
            return 0;
        }
    }


    public virtual async Task<bool> UpdateAsync(T item, CancellationToken Cancel = default)
    {
        if (!await UpdateEntity(item, Cancel).ConfigureAwait(false)) return false;

        try
        {
            var changesCount = await _Db.SaveChangesAsync(Cancel).ConfigureAwait(false);
            var result = changesCount > 0;

            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }



    public virtual async Task<int> UpdateRangeAsync(IEnumerable<T> items, CancellationToken Cancel = default)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        foreach (var item in items)
            await UpdateEntity(item, Cancel).ConfigureAwait(false);

        try
        {
            return await _Db.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }
        catch (Exception)
        {
            return 0;
        }
    }

    private async Task<bool> UpdateEntity(T item, CancellationToken Cancel)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        var localItem = await GetByIdAsync(item.Id, Cancel).ConfigureAwait(false);
        if (localItem == null)
            return false;

        if (!ReferenceEquals(localItem, item))
            _Db.Entry(localItem).State = EntityState.Detached;

        _Db.Entry(item).State = EntityState.Modified;
        return true;
    }



    public virtual async Task<bool> UpdatePropertyAsync(T item, Expression<Func<T, object>> property, CancellationToken Cancel = default) =>
        await UpdatePropertiesAsync(item, new[] { property }, Cancel);


    public virtual async Task<bool> UpdatePropertiesAsync(T item, IEnumerable<Expression<Func<T, object>>> properties, CancellationToken Cancel = default) => 
        await UpdateAsync(item, Cancel).ConfigureAwait(false);


    public virtual async Task<bool> DeleteAsync(TKey id, CancellationToken Cancel = default)
    {

        var item = Items.FirstOrDefault(i => Equals(id, i.Id));
        if (item == null)
        {
            return false;
        }

        MarkDeletedOrDelete(new[] { item });

        try
        {
            return await _Db.SaveChangesAsync(Cancel).ConfigureAwait(false) > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }


    public virtual async Task<int> DeleteRangeAsync(IEnumerable<TKey> ids, CancellationToken Cancel = default)
    {
        var existing = await Items.Select(item => item.Id).Where(id => ids.Contains(id)).ToArrayAsync(Cancel);

        var itemsToDelete = existing
            .Select(id =>
            {
                var entity = Items.FirstOrDefault(i => Equals(id, i.Id));
                if (entity == null)
                {
                    var dbEntity = new T { Id = id };
                    _Db.Attach(dbEntity);
                    return dbEntity;
                }
                return entity;

            });

        MarkDeletedOrDelete(itemsToDelete);

        try
        {
            return await _Db.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }
        catch (Exception)
        {
            return 0;
        }
    }

    #endregion


    /// <summary> Пометить на удаление или удалить окончательно </summary>
    protected virtual void MarkDeletedOrDelete(IEnumerable<T> items)
    {
        if (IsDeletedEntity)
        {
            foreach (var item in items)
            {
                var deletedEntity = (IDeletedEntityBase<int>)item;
                _Db.Entry(deletedEntity).State = EntityState.Unchanged;
                deletedEntity.IsDeleted = true;
                _Db.Entry(deletedEntity)
                    .Property(delItem => delItem.IsDeleted).IsModified = true;
            }
        }
        else
            foreach (var item in items)
                _Db.Entry(item).State = EntityState.Deleted;
    }

    public Task<T?> GetOne(Expression<Func<T, bool>> Match, CancellationToken cancel = default) => Items.FirstOrDefaultAsync(Match, cancellationToken: cancel);
}