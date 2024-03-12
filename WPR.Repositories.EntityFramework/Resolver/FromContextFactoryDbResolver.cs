using System;
using Microsoft.EntityFrameworkCore;

namespace WPR.Repositories.EntityFramework.Resolver;

/// <summary>
/// Для одной БД в приложении.
/// Работает с сервисом фабрики контекста БД
/// </summary>
internal sealed class FromContextFactoryDbResolver<TDb>(IDbContextFactory<TDb> DbContextFactory) : IDbResolver where TDb : DbContext
{

    public DbContext GetDbContext<T>() => DbContextFactory.CreateDbContext();

    public DbContext GetDbContext(Type EntityType) => DbContextFactory.CreateDbContext();
}