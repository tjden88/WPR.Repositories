using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WPR.Entities.Abstractions.Db;
using WPR.Repositories.Abstractions.Base;

namespace WPR.Repositories.Abstractions.Db;

/// <summary>
/// Репозиторий сущностей БД
/// </summary>
/// <typeparam name="TDbEntity">Сущность БД</typeparam>
public interface IDbRepository<TDbEntity> : IRepository<TDbEntity, int> where TDbEntity : IDbEntity
{
    /// <summary>
    /// Получить единственную сущность по предикату
    /// </summary>
    /// <param name="Match">Предикат выборки</param>
    /// <param name="cancel">Токен отмены</param>
    /// <returns>null, если ничего не нашлось, либо первая подходящая</returns>
    Task<TDbEntity?> GetOne(Expression<Func<TDbEntity, bool>> Match, CancellationToken cancel = default);
}