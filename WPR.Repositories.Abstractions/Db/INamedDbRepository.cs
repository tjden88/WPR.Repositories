using WPR.Entities.Abstractions.Db;
using WPR.Repositories.Abstractions.Base;

namespace WPR.Repositories.Abstractions.Db;

/// <summary>
/// Репозиторий именованных сущностей БД
/// </summary>
/// <typeparam name="TDbEntity">Именованная сущность БД</typeparam>

public interface INamedDbRepository<TDbEntity> : IDbRepository<TDbEntity>, INamedRepository<TDbEntity, int> where TDbEntity : INamedDbEntity;