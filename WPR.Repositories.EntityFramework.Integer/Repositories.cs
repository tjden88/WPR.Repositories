using WPR.Repositories.EntityFramework.Base.Repositories;
using WPR.Repositories.EntityFramework.Base.Resolver;
using WPR.Repositories.Integer;

namespace WPR.Repositories.EntityFramework.Integer;

public class DbRepositoryInt<T>(IDbResolver DbResolver) : DbRepositoryBase<T, int>(DbResolver) where T : Entity, new();
public class DbNamedRepositoryInt<T>(IDbResolver DbResolver) : DbNamedRepositoryBase<T, int>(DbResolver) where T : NamedEntity, new();
public class DbDeletedRepositoryInt<T>(IDbResolver DbResolver) : DbDeletedRepositoryBase<T, int>(DbResolver) where T : DeletedEntity, new();