using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WPR.Repositories.Abstractions.Db;

namespace WPR.Repositories.EntityFramework.Resolver;

public static class RepositoryRegistrator
{
    private static bool _AddDbRepositoriesWasCalled;

    /// <summary>
    /// Регистрация необходимых репозиториев БД.
    /// Работает с несколькими контекстами баз данных
    /// </summary>
    public static IServiceCollection AddDbRepositories(this IServiceCollection services)
    {
        if (_AddDbRepositoriesWasCalled)
            throw new InvalidOperationException("Повторное добавление репозиториев БД невозможно");
        _AddDbRepositoriesWasCalled = true;

        MultipleDbResolver.Services = services;
        return services
                .AddScoped<IDbResolver, MultipleDbResolver>()
                .AddRepositories()
            ;
    }

    /// <summary>
    /// Регистрация необходимых репозиториев БД.
    /// Работает с фабрикой определённой и единственной базой данных
    /// </summary>
    public static IServiceCollection AddDbRepositories<TDb>(this IServiceCollection services) where TDb : DbContext
    {
        if (_AddDbRepositoriesWasCalled)
            throw new InvalidOperationException("Повторное добавление репозиториев БД невозможно");
        _AddDbRepositoriesWasCalled = true;

        return services
                .AddScoped<IDbResolver, FromContextFactoryDbResolver<TDb>>()
                .AddRepositories()
            ;
    }


    private static IServiceCollection AddRepositories(this IServiceCollection services) => services
        .AddScoped(typeof(IDbRepository<>), typeof(DbRepository<>))
        .AddScoped(typeof(INamedDbRepository<>), typeof(DbNamedRepository<>))
        .AddScoped(typeof(IDeletedDbRepository<>), typeof(DbDeletedRepository<>))
    ;
}