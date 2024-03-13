using System;
using System.Linq.Expressions;

namespace WPR.Repositories.Base.Models;

/// <summary>
/// Сведения о сортировке при формировании страницы
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public record PageOrderInfo<T> : IPageOrderInfo<T>
{
    /// <summary>
    /// Сведения о сортировке при формировании страницы
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    /// <param name="OrderExpression">Выражение сортировки объекта</param>
    /// <param name="IsAscending">орядок сортировки по возрастанию (по умолчанию - true)</param>
    public PageOrderInfo(Expression<Func<T, object>> OrderExpression, bool IsAscending = true)
    {
        this.OrderExpression = OrderExpression;
        this.IsAscending = IsAscending;
    }

    /// <summary>Выражение сортировки объекта</summary>
    public Expression<Func<T, object>> OrderExpression { get; }


    /// <summary>орядок сортировки по возрастанию (по умолчанию - true)</summary>
    public bool IsAscending { get; }

    public void Deconstruct(out Expression<Func<T, object>> orderExpression, out bool Ascending)
    {
        orderExpression = OrderExpression;
        Ascending = IsAscending;
    }
}