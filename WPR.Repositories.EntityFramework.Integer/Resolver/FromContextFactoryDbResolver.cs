using System;
using Microsoft.EntityFrameworkCore;
using WPR.Repositories.EntityFramework.Base.Resolver;

namespace WPR.Repositories.EntityFramework.Integer.Resolver;

/// <summary>
/// Для одной БД в приложении.
/// Работает с сервисом фабрики контекста БД
/// </summary>
internal sealed class FromContextFactoryDbResolver<TDb>(IDbContextFactory<TDb> DbContextFactory) : IDbResolver where TDb : DbContext
{

    public DbContext GetDbContext<T>() => DbContextFactory.CreateDbContext();

    public DbContext GetDbContext(Type EntityType) => DbContextFactory.CreateDbContext();
}