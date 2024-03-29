﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WPR.Repositories.Base.Entities;
using WPR.Repositories.Base.Models;
using WPR.Repositories.Base.Repositories;
using WPR.Repositories.EntityFramework.Base.Resolver;

namespace WPR.Repositories.EntityFramework.Base.Repositories;

/// <summary>
/// Репозиторий именованных сущностей БД
/// </summary>
/// <typeparam name="T">Именованная сущность</typeparam>
/// <typeparam name="TKey">Тип первичного ключа</typeparam>
public class DbNamedRepositoryBase<T, TKey>(IDbResolver DbResolver) : DbRepositoryBase<T, TKey>(DbResolver), INamedRepositoryBase<T, TKey> where T : class, INamedEntityBase<TKey>, new() where TKey : IComparable<TKey>
{
    public virtual Task<bool> ExistNameAsync(string Name, CancellationToken Cancel = default) =>
        Task.FromResult(Items.Any(item => item.Name == Name));

    public virtual async Task<T?> GetByNameAsync(string Name, CancellationToken Cancel = default) =>
        await GetOne(i => i.Name == Name, Cancel).ConfigureAwait(false);

    public async Task<bool> UpdateNameAsync(TKey id, string newName, CancellationToken Cancel = default) =>
        await UpdatePropertyAsync(new T { Id = id, Name = newName }, item => item.Name, Cancel)
            .ConfigureAwait(false);
}