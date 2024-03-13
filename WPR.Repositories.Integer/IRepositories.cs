using WPR.Entities.Abstractions.Base;
using WPR.Repositories.Abstractions.Base;

namespace WPR.Repositories.Integer;

public interface IRepository<T> : IRepository<T, int> where T : IEntity<int>;

public interface INamedRepository<T> : INamedRepository<T, int> where T : INamedEntity<int>;

public interface IDeletedRepository<T> : IRepository<T, int> where T : IDeletedEntity<int>;