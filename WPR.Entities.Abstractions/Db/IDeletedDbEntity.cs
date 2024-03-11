using WPR.Entities.Abstractions.Base;

namespace WPR.Entities.Abstractions.Db;

/// <summary> Удаляемая сущность Базы данных с Id - int </summary>
public interface IDeletedDbEntity : IDbEntity, IDeletedEntity<int>;