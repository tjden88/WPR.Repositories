using Microsoft.Extensions.DependencyInjection;
using WPR.Repositories.Abstractions.Db;

namespace WPR.Repositories.EntityFramework;

public static class RepositoryRegistrator
{
    /// <summary>
    /// Зарегистрировать интерфейсы репозиториев базы данных с реализацией EntityFramework
    /// </summary>
    public static IServiceCollection AddDbRepositories(this IServiceCollection services) => services
        .AddScoped(typeof(IDbRepository<>), typeof(DbRepository<>))
        .AddScoped(typeof(INamedDbRepository<>), typeof(DbNamedRepository<>))
        .AddScoped(typeof(IDeletedDbRepository<>), typeof(DbDeletedRepository<>))
        
    ;
}