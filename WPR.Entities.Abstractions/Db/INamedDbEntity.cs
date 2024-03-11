using WPR.Entities.Abstractions.Base;

namespace WPR.Entities.Abstractions.Db;

/// <summary> Именованная сущность Базы данных с Id - int </summary>
public interface INamedDbEntity : IDbEntity, INamedEntity<int>;