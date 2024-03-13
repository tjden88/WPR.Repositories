using WPR.Entities.Base;
using WPR.Repositories.EntityFramework.Resolver;

namespace WPR.Repositories.EntityFramework.Integer;

public class DbRepository<T>(IDbResolver DbResolver) : DbRepository<T, int>(DbResolver) where T : Entity<int>, new();
public class DbNamedRepository<T>(IDbResolver DbResolver) : DbNamedRepository<T, int>(DbResolver) where T : NamedEntity<int>, new();
public class DbDeletedRepository<T>(IDbResolver DbResolver) : DbDeletedRepository<T, int>(DbResolver) where T : DeletedEntity<int>, new();