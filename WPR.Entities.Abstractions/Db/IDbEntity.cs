using WPR.Entities.Abstractions.Base;

namespace WPR.Entities.Abstractions.Db;

/// <summary> Сущность Базы данных с Id - int </summary>
public interface IDbEntity : IEntity<int>;