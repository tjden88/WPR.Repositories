﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WPR.Repositories.WebApi;

public class WebRepository<T> : BaseClient, IRepository<T> where T : IEntity
{
    public WebRepository(HttpClient Client, ILogger<WebRepository<T>> Logger) : base(Client, $"api/{typeof(T).Name}", Logger) { }

    protected WebRepository(HttpClient Client, string Address, ILogger<WebRepository<T>> Logger) : base(Client, Address, Logger) { }


    public virtual IQueryable<T> Items
    {
        get
        {
            var entities = Get<IEnumerable<T>>($"{Address}");
            return entities is null 
                ? Enumerable.Empty<T>().AsQueryable() 
                : entities.AsQueryable();
        }
    }


    public virtual IQueryable<T> Get(Expression<Func<T, bool>> Filter)
    {
        if (Filter == null)
            throw new ArgumentNullException(nameof(Filter));

        var queryFilter = new QueryExpressionDto(Filter);
        try
        {
            var response = Post($"{Address}/get", queryFilter);

            var entities = response?.Content.ReadFromJsonAsync<T[]>().Result;
            return entities != null
                ? entities.AsQueryable()
                : Enumerable.Empty<T>().AsQueryable();

        }
        catch (Exception e)
        {
            Logger.LogError(e, "Ошибка выборки элементов в WebAPI {0} : {1}", typeof(T).Name, e.Message);
            return Enumerable.Empty<T>().AsQueryable();
        }
    }

    public async System.Threading.Tasks.Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> Filter, CancellationToken Cancel = default)
    {
        if (Filter == null)
            throw new ArgumentNullException(nameof(Filter));

        var queryFilter = new QueryExpressionDto(Filter);
        try
        {
            var response = await PostAsync($"{Address}/get", queryFilter, Cancel);

            var entities = await response.Content.ReadFromJsonAsync<T[]>(cancellationToken: Cancel)

                .ConfigureAwait(false);
            return entities ?? Enumerable.Empty<T>();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Ошибка выборки элементов в WebAPI {0} : {1}", typeof(T).Name, e.Message);
            return Enumerable.Empty<T>();
        }
    }


    public async Task<IPage<T>> GetPage(int PageIndex, int PageSize, Expression<Func<T, object>> OrderExpression = null, bool Ascending = true, CancellationToken Cancel = default) =>
        await GetPage(new PageFilter<T>
        {
            PageIndex = PageIndex,
            PageSize = PageSize,
            OrderBy = OrderExpression is null
                ? null
                : new PageOrderInfo<T>
                {
                    Ascending = Ascending,
                    OrderExpression = OrderExpression,
                }
        }, Cancel)
            .ConfigureAwait(false);


    public async Task<IPage<T>> GetPage(IPageFilter<T> Filter, CancellationToken Cancel = default)
    {
        if (Filter is null)
            throw new ArgumentNullException(nameof(Filter));

        var filter = Filter.Filter is null ? null : new QueryExpressionDto(Filter.Filter);

        var orderBy = Filter.OrderBy?.OrderExpression is null
            ? null
            : new PageQueryDto.OrderQuery(
                new QueryExpressionDto(Filter.OrderBy.OrderExpression)
                , Filter.OrderBy.Ascending);


        var pageQuery = new PageQueryDto
        {
            Order = orderBy,
            Filter = filter,
            PageIndex = Filter.PageIndex,
            PageSize = Filter.PageSize,
        };

        if (Filter.ThenOrderBy?.Any() == true)
            pageQuery.ThenOrder = Filter
                .ThenOrderBy
                .Select(thenOrder =>
                    new PageQueryDto.OrderQuery(new QueryExpressionDto(thenOrder.OrderExpression), thenOrder.Ascending))
                .ToList();

        try
        {
            var response = await PostAsync($"{Address}/page", pageQuery, Cancel).ConfigureAwait(false);
            var page = await response.Content.ReadFromJsonAsync<Page<T>>(cancellationToken: Cancel).ConfigureAwait(false);
            return page;
        }
        catch (Exception e)
        {
            Logger.LogError("Не удалось получить страницу выборки элементов в WebAPI {0} : {1}", typeof(T).Name, e.Message);
            return new Page<T>(Enumerable.Empty<T>(), 0, Filter.PageIndex, Filter.PageSize);
        }
    }

    public virtual async Task<int> CountAsync(CancellationToken Cancel = default) => await GetAsync<int>($"{Address}/count", Cancel).ConfigureAwait(false);


    public virtual async Task<bool> ExistAsync(int id, CancellationToken Cancel = default) => await GetAsync<bool>($"{Address}/exist/{id}", Cancel).ConfigureAwait(false);


    public virtual async Task<T> GetByIdAsync(int id, CancellationToken Cancel = default) => await GetAsync<T>($"{Address}/{id}", Cancel).ConfigureAwait(false);


    public virtual async Task<T> AddAsync(T item, CancellationToken Cancel = default)
    {
        var response = await PostAsync($"{Address}", item, Cancel).ConfigureAwait(false);
        return response?.IsSuccessStatusCode == true
            ? await response.Content
                .ReadFromJsonAsync<T>(cancellationToken: Cancel)
                .ConfigureAwait(false)
            : default;
    }


    public virtual async Task<int> AddRangeAsync(IEnumerable<T> items, CancellationToken Cancel = default)
    {
        var response = await PostAsync($"{Address}/range", items, Cancel).ConfigureAwait(false);
        return response?.IsSuccessStatusCode == true
            ? await response.Content
                .ReadFromJsonAsync<int>(cancellationToken: Cancel)
                .ConfigureAwait(false)
            : 0;
    }


    public virtual async Task<bool> UpdateAsync(T item, CancellationToken Cancel = default)
    {
        var response = await PutAsync($"{Address}", item, Cancel).ConfigureAwait(false);
        return response?.IsSuccessStatusCode == true;
    }


    public virtual async Task<int> UpdateRangeAsync(IEnumerable<T> items, CancellationToken Cancel = default)
    {
        var response = await PutAsync($"{Address}/range", items, Cancel).ConfigureAwait(false);
        return response?.IsSuccessStatusCode == true
            ? await response.Content
                .ReadFromJsonAsync<int>(cancellationToken: Cancel)
                .ConfigureAwait(false)
            : 0;
    }


    public virtual async Task<bool> UpdatePropertyAsync(T item, Expression<Func<T, object>> property, CancellationToken Cancel = default) => 
        await UpdatePropertiesAsync(item, new[] {property}, Cancel).ConfigureAwait(false);


    public virtual async Task<bool> UpdatePropertiesAsync(T item, IEnumerable<Expression<Func<T, object>>> properties, CancellationToken Cancel = default)
    {
        if(item == null) 
            throw new ArgumentNullException(nameof(item));
        if(properties == null)
            throw new ArgumentNullException(nameof(properties));

        var dto = new EntityExpressionDto<T>
        {
            Item = item,
            Expressions = properties.Select(p => new QueryExpressionDto(p)),
        };

        var response = await PutAsync($"{Address}/properties", dto, Cancel).ConfigureAwait(false);
        return response?.IsSuccessStatusCode == true;
    }


    public virtual async Task<bool> DeleteAsync(int id, CancellationToken Cancel = default)
    {
        var response = await DeleteAsync($"{Address}/{id}", Cancel).ConfigureAwait(false);
        return response?.IsSuccessStatusCode == true;
    }


    public virtual async Task<int> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken Cancel = default)
    {
        var response = await PostAsync($"{Address}/deleterange", ids, Cancel).ConfigureAwait(false);
        return response?.IsSuccessStatusCode == true
            ? await response.Content
                .ReadFromJsonAsync<int>(cancellationToken: Cancel)
                .ConfigureAwait(false)
            : 0;
    }
}