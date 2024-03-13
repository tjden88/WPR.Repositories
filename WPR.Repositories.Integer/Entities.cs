using WPR.Repositories.Base.Entities;

namespace WPR.Repositories.Integer;

public interface IEntity : IEntityBase<int>;
public interface INamedEntity : INamedEntityBase<int>, IEntity;
public interface IDeletedEntity : IDeletedEntityBase<int>, IEntity;

