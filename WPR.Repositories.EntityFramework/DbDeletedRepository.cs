using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WPR.Entities.Abstractions.Db;
using WPR.Entities.Db;
using WPR.Repositories.Abstractions.Db;
using WPR.Repositories.EntityFramework.Resolver;

namespace WPR.Repositories.EntityFramework;

/// <summary>
/// Репозиторий удалённых сущностей БД
/// </summary>
/// <typeparam name="T">IDeletedEntity</typeparam>
public class DbDeletedRepository<T>(IDbResolver DbResolver) : DbRepository<T>(DbResolver), IDeletedDbRepository<T> where T : DbEntity, IDeletedDbEntity, new()
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


    public async Task<T?> RestoreAsync(int id, CancellationToken Cancel = default)
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