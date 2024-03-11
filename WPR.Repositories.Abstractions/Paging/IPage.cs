namespace WPR.Repositories.Abstractions.Paging;

/// <summary>
/// Страница ограниченной выборки данных
/// </summary>
public interface IPage
{
    /// <summary>
    /// Номер страницы (начиная с единицы)
    /// </summary>
    int PageIndex { get; }

    /// <summary>
    /// Размер страницы
    /// </summary>
    int PageSize { get; }

    /// <summary>
    /// Всего страниц
    /// </summary>
    int TotalPagesCount { get; }

    /// <summary>
    /// Есть предыдущая страница
    /// </summary>
    bool HasPreviousPage { get; }

    /// <summary>
    /// Есть следующая страница
    /// </summary>
    bool HasNextPage { get; }

    /// <summary>
    /// Всего больше одной страницы
    /// </summary>
    bool HasManyPages { get; }
}