using WPR.Repositories.Base.Entities;
using WPR.Repositories.Base.Repositories;

namespace WPR.Repositories.Integer;

public interface IRepository<T> : IRepositoryBase<T, int> where T : IEntityBase<int>;

public interface INamedRepository<T> : INamedRepositoryBase<T, int> where T : INamedEntityBase<int>;

public interface IDeletedRepository<T> : IRepositoryBase<T, int> where T : IDeletedEntityBase<int>;