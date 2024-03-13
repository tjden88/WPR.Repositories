using WPR.Repositories.Base.Entities;
using WPR.Repositories.EntityFramework.Base.Repositories;
using WPR.Repositories.EntityFramework.Base.Resolver;
using WPR.Repositories.Integer;

namespace WPR.Repositories.EntityFramework.Integer;

public class DbRepositoryInt<T>(IDbResolver DbResolver) : DbRepositoryBase<T, int>(DbResolver), IRepository<T> where T : class, IEntityBase<int>, new();
public class DbNamedRepositoryInt<T>(IDbResolver DbResolver) : DbNamedRepositoryBase<T, int>(DbResolver), INamedRepository<T> where T : class, INamedEntityBase<int>, new();
public class DbDeletedRepositoryInt<T>(IDbResolver DbResolver) : DbDeletedRepositoryBase<T, int>(DbResolver), IDeletedRepository<T> where T : class, IDeletedEntityBase<int>, new();