using System.Collections.Generic;

namespace WPR.Repositories.Abstractions.Paging;

/// <summary>
/// Страница данных с коллекцией элементов
/// </summary>
/// <typeparam name="T">Тип данных элемента</typeparam>
public interface IPage<out T> : IPage
{
    /// <summary> Сущности страницы </summary>
    IEnumerable<T> Items { get; }

    /// <summary>
    /// Общее количество сущностей в выборке
    /// </summary>
    int TotalItemsCount { get; }

}