using WPR.Repositories.EntityFramework.Base.Resolver;
using WPR.Repositories.Integer;

namespace WPR.Repositories.EntityFramework.Integer;

public class DbRepository<T>(IDbResolver DbResolver) : DbRepository<T, int>(DbResolver) where T : Entity, new();
public class DbNamedRepository<T>(IDbResolver DbResolver) : DbNamedRepository<T, int>(DbResolver) where T : NamedEntity, new();
public class DbDeletedRepository<T>(IDbResolver DbResolver) : DbDeletedRepository<T, int>(DbResolver) where T : DeletedEntity, new();