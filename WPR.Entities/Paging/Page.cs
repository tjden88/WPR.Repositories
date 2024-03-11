using System.Collections.Generic;
using System;
using WPR.Repositories.Abstractions.Paging;

namespace WPR.Entities.Paging;

/// <summary>
/// Реализация интерфейса постраничной выборки элементов репозиториев
/// </summary>
public record Page<T> : IPage<T>
{
    /// <summary>
    /// Реализация интерфейса постраничной выборки элементов репозиториев
    /// </summary>
    public Page(IEnumerable<T> Items, int TotalItemsCount, int PageIndex, int PageSize)
    {
        this.Items = Items;
        this.TotalItemsCount = TotalItemsCount;
        this.PageIndex = PageIndex;
        this.PageSize = PageSize;
    }

    public int TotalPagesCount => PageSize < 1
        ? 0
        : (int)Math.Ceiling((double)TotalItemsCount / PageSize + 1) - 1;


    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPagesCount;
    public bool HasManyPages => TotalPagesCount > 1;


    public IEnumerable<T> Items { get; }
    public int TotalItemsCount { get; }
    public int PageIndex { get; }
    public int PageSize { get; }

}