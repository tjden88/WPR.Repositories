using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WPR.Repositories.EntityFramework.Base.Resolver;
using WPR.Repositories.EntityFramework.Integer.Resolver;
using WPR.Repositories.Integer;

namespace WPR.Repositories.EntityFramework.Integer;

public static class RepositoryRegistrator
{
    private static bool _AddDbRepositoriesWasCalled;

    /// <summary>
    /// Зарегистрировать интерфейсы репозиториев базы данных с реализацией EntityFramework
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
    /// Зарегистрировать интерфейсы репозиториев базы данных с реализацией EntityFramework
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
        .AddScoped(typeof(IRepository<>), typeof(DbRepositoryInt<>))
        .AddScoped(typeof(INamedRepository<>), typeof(DbNamedRepositoryInt<>))
        .AddScoped(typeof(IDeletedRepository<>), typeof(DbDeletedRepositoryInt<>))
    ;
}