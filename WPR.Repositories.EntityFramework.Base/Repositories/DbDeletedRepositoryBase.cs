using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WPR.Repositories.Base.Entities;
using WPR.Repositories.Base.Models;
using WPR.Repositories.Base.Repositories;
using WPR.Repositories.EntityFramework.Base.Resolver;

namespace WPR.Repositories.EntityFramework.Base.Repositories;

/// <summary>
/// Репозиторий удалённых сущностей БД
/// </summary>
/// <typeparam name="T">IDeletedEntity</typeparam>
/// <typeparam name="TKey">Тип первичного ключа</typeparam>
public class DbDeletedRepositoryBase<T, TKey>(IDbResolver DbResolver) : DbRepositoryBase<T, TKey>(DbResolver), IDeletedRepositoryBase<T, TKey> where T : class, IDeletedEntityBase<TKey>, new() where TKey : IComparable<TKey>
{
    protected override IQueryable<T> Items => Set.Where(item => item.IsDeleted);


    public override Task<T?> AddAsync(T item, CancellationToken Cancel = default)
    {
        item.IsDeleted = true;
        return base.AddAsync(item, Cancel);
    }


    public override Task<int> AddRangeAsync(IEnumerable<T> items, CancellationToken Cancel = default)
    {
        var deletedEntities = items as T[] ?? items.ToArray();
        foreach (var deletedEntity in deletedEntities)
            deletedEntity.IsDeleted = true;

        return base.AddRangeAsync(deletedEntities, Cancel);
    }


    public async Task<T?> RestoreAsync(TKey id, CancellationToken Cancel = default)
    {
        var entity = await GetByIdAsync(id, Cancel).ConfigureAwait(false);

        if (entity is null)
            return null;

        entity.IsDeleted = false;
        var restored = await UpdatePropertyAsync(entity, item => item.IsDeleted, Cancel);

        return restored
            ? entity
            : null;
    }
}