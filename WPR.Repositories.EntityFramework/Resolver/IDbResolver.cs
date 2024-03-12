using System;
using Microsoft.EntityFrameworkCore;

namespace WPR.Repositories.EntityFramework.Resolver;

/// <summary>
/// Определяет, к какому комтексту БД принадлежит определённая сущность и запрашивает его из контейнера сервисов
/// </summary>
public interface IDbResolver
{
    /// <summary>
    /// Получить контекст БД для связанной сущности
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    DbContext GetDbContext<T>();

    /// <summary>
    /// Получить контекст БД для связанной сущности
    /// </summary>
    /// <param name="EntityType">Тип сущности</param>
    /// <exception cref="InvalidOperationException"></exception>
    DbContext GetDbContext(Type EntityType);
}