
using WPR.Repositories.Base.Models;

namespace WPR.Repositories.Integer;

public abstract class Entity : EntityBase<int>;
public abstract class NamedEntity : NamedEntityBase<int>;
public abstract class DeletedEntity : DeletedEntityBase<int>;

