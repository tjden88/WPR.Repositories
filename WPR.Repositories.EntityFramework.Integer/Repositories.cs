using WPR.Repositories.EntityFramework.Base.Repositories;
using WPR.Repositories.EntityFramework.Base.Resolver;
using WPR.Repositories.Integer;

namespace WPR.Repositories.EntityFramework.Integer;

public class DbRepositoryInt<T>(IDbResolver DbResolver) : DbRepositoryBase<T, int>(DbResolver), IRepository<T> where T : class, IEntity, new();
public class DbNamedRepositoryInt<T>(IDbResolver DbResolver) : DbNamedRepositoryBase<T, int>(DbResolver), INamedRepository<T> where T : class, INamedEntity, new();
public class DbDeletedRepositoryInt<T>(IDbResolver DbResolver) : DbDeletedRepositoryBase<T, int>(DbResolver), IDeletedRepository<T> where T : class, IDeletedEntity, new();