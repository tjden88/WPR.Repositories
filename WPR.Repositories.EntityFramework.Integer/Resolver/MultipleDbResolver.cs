using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WPR.Entities.Abstractions.Base;
using WPR.Repositories.EntityFramework.Resolver;

namespace WPR.Repositories.EntityFramework.Integer.Resolver;

/// <summary>
/// Выдаёт контекст из контейнера в зависимости от типа сущности
/// </summary>
internal sealed class MultipleDbResolver : IDbResolver
{
    internal static IServiceCollection? Services { get; set; }

    private readonly IServiceProvider _ServiceProvider;
    private readonly ILogger<MultipleDbResolver> _Logger;

    private static Dictionary<Type, Type>? _Types;


    public MultipleDbResolver(IServiceProvider ServiceProvider)
    {
        if (Services is null)
            throw new ArgumentNullException(nameof(Services));

        _ServiceProvider = ServiceProvider;
        _Logger ??= _ServiceProvider.GetRequiredService<ILogger<MultipleDbResolver>>();

        if (_Types is null)
            InitializeEntities(ServiceProvider, Services);
    }


    /// <summary>
    /// Получить контекст БД для связанной сущности
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public DbContext GetDbContext<T>() => GetDbContext(typeof(T));

    /// <summary>
    /// Получить контекст БД для связанной сущности
    /// </summary>
    /// <param name="EntityType">Тип сущности</param>
    /// <exception cref="InvalidOperationException"></exception>
    public DbContext GetDbContext(Type EntityType)
    {

        if (_Types!.TryGetValue(EntityType, out var dbType))
        {
            return (DbContext)_ServiceProvider.GetRequiredService(dbType);
        }

        _Logger.LogError("Тип сущности {0} не найден для зарегистрированных в контейнере сервисов контекстах БД", EntityType);

        throw new InvalidOperationException(
            $"Тип сущности {EntityType} не найден для зарегистрированных в контейнере сервисов контекстах БД");
    }

    private void InitializeEntities(IServiceProvider ServiceProvider, IServiceCollection ServiceCollection)
    {
        _Logger.LogDebug("Инициализация сущностей баз данных");
        _Types = new();
        var timer = Stopwatch.StartNew();
        using var scope = ServiceProvider.CreateScope();

        var allDbContextsTypes = ServiceCollection
            .Where(sd => sd.ServiceType.IsAssignableTo(typeof(DbContext)))
            .Select(sd => sd.ServiceType);


        foreach (var contextType in allDbContextsTypes)
        {
            var dbContext = (DbContext)scope.ServiceProvider.GetRequiredService(contextType);
            _Logger.LogDebug("Инициализация типов сущностей в базе данных {0}", dbContext);

            var entityTypes = dbContext.Model
                .GetEntityTypes()
                .Select(et => et.ClrType)
                .Where(t => t.IsAssignableTo(typeof(IEntity<int>)));

            foreach (var entityType in entityTypes)
                _Types.Add(entityType, contextType);

        }

        _Logger.LogDebug("Инициализация сущностей баз данных завершена за {0} мс.", timer.ElapsedMilliseconds);

    }
}