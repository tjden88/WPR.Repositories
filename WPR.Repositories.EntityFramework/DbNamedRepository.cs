using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WPR.Entities.Base;
using WPR.Repositories.Abstractions.Base;
using WPR.Repositories.EntityFramework.Resolver;

namespace WPR.Repositories.EntityFramework;

/// <summary>
/// Репозиторий именованных сущностей БД
/// </summary>
/// <typeparam name="T">Именованная сущность</typeparam>
/// <typeparam name="TKey">Тип первичного ключа</typeparam>
public class DbNamedRepository<T, TKey>(IDbResolver DbResolver) : DbRepository<T, TKey>(DbResolver), INamedRepository<T, TKey> where T : NamedEntity<TKey>, new() where TKey : IComparable<TKey>
{
    public virtual Task<bool> ExistNameAsync(string Name, CancellationToken Cancel = default) =>
        Task.FromResult(Items.Any(item => item.Name == Name));

    public virtual async Task<T?> GetByNameAsync(string Name, CancellationToken Cancel = default) =>
        await GetOne(i => i.Name == Name, Cancel).ConfigureAwait(false);

    public async Task<bool> UpdateNameAsync(TKey id, string newName, CancellationToken Cancel = default) =>
        await UpdatePropertyAsync(new T { Id = id, Name = newName }, item => item.Name, Cancel)
            .ConfigureAwait(false);
}