using System;

namespace BookHub.Models.Dispatching;

/// <summary>
/// Описывает диспатчера событий.
/// </summary>
public interface IEventDispatcher
{
    /// <summary>
    /// Регистрирует обработчик события.
    /// </summary>
    /// <param name="eventHandler">
    /// Обработчик события типа <typeparamref name="T"/>.
    /// </param>
    /// <typeparam name="T">
    /// Тип события.
    /// </typeparam>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="eventHandler"/> - <see langword="null"/>.
    /// </exception>
    void Register<T>(Action<T> eventHandler) where T : IDomainEvent;

    /// <summary>
    /// Публикует событие типа <typeparamref name="T"/>.
    /// </summary>
    /// <param name="publishingEvent">
    /// Публикуемое событие.
    /// </param>
    /// <typeparam name="T">
    /// Тип события.
    /// </typeparam>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="publishingEvent"/> - <see langword="null"/>.
    /// </exception>
    void Publish<T>(T publishingEvent) where T : IDomainEvent;
}
