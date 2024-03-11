﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WPR.Entities.Db;
using WPR.Repositories.Abstractions.Db;

namespace WPR.Repositories.EntityFramework;

/// <summary>
/// Репозиторий именованных сущностей БД
/// </summary>
/// <typeparam name="T">Именованная сущность</typeparam>
public class DbNamedRepository<T>(DbContext Db) : DbRepository<T>(Db), INamedDbRepository<T> where T : NamedDbEntity, new()
{
    public virtual Task<bool> ExistNameAsync(string Name, CancellationToken Cancel = default) =>
        Task.FromResult(Items.Any(item => item.Name == Name));

    public virtual async Task<T?> GetByNameAsync(string Name, CancellationToken Cancel = default) =>
        await GetOne(i => i.Name == Name, Cancel).ConfigureAwait(false);

    public async Task<bool> UpdateNameAsync(int id, string newName, CancellationToken Cancel = default) =>
        await UpdatePropertyAsync(new T { Id = id, Name = newName }, item => item.Name, Cancel)
            .ConfigureAwait(false);
}