using System;
using System.Linq.Expressions;

namespace WPR.Repositories.Base.Paging;

/// <summary>
/// Сведения о сортировке при формировании страницы IPage
/// </summary>
public interface IPageOrderInfo<T>
{
    /// <summary>
    /// Выражение сортировки объекта
    /// </summary>
    Expression<Func<T, object>> OrderExpression { get; }


    /// <summary>
    /// Порядок сортировки по возрастанию или убыванию
    /// </summary>
    bool IsAscending { get; }
}