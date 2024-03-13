using WPR.Repositories.Base.Models;

namespace WPR.Repositories.Integer;

public class Entity : EntityBase<int>, IEntity;
public class NamedEntity : NamedEntityBase<int>, INamedEntity;
public class DeletedEntity : DeletedEntityBase<int>, IDeletedEntity;

