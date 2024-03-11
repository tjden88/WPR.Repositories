using WPR.Entities.Abstractions.Db;
using WPR.Repositories.Abstractions.Base;

namespace WPR.Repositories.Abstractions.Db;

/// <summary>
/// Репозиторий удаляемых сущностей БД
/// </summary>
/// <typeparam name="TDbEntity">Удаляемая сущность БД</typeparam>

public interface IDeletedDbRepository<TDbEntity> : IDeletedRepository<TDbEntity, int> where TDbEntity : IDeletedDbEntity;